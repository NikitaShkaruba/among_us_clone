using System.Collections;
using System.Linq;
using AmongUsClone.Server.Game.Interactions;
using AmongUsClone.Server.Game.Maps.Surveillance;
using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;
using Random = System.Random;

namespace AmongUsClone.Server.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "LobbyGamePhase", menuName = "LobbyGamePhase")]
    public class LobbyGamePhase : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Lobby lobby;

        public const int SecondsForGameLaunch = GameConfiguration.SecondsForGameLaunch;

        private bool gameStartRequested;

        public void Initialize()
        {
            lobby = FindObjectOfType<Lobby>();

            gameStartRequested = false;
        }

        public void ConnectPlayer(int playerId, string playerName)
        {
            Vector2 playerPosition = lobby.playerSpawnPrototypes[playerId].transform.position;
            ServerPlayer serverPlayer = Instantiate(playerPrefab, playerPosition, Quaternion.identity).GetComponent<ServerPlayer>();
            serverPlayer.gameObject.name = $"Player{playerId}";
            serverPlayer.transform.parent = lobby.playersContainer.transform;

            PlayerColor playerColor = PlayerColors.TakeFreeColor(playerId);
            bool isLookingRight = !lobby.playerSpawnPrototypes[playerId].spriteRenderer.flipX;
            bool isLobbyHost = playerId == PlayersManager.MinPlayerId;
            serverPlayer.Initialize(playerId, playerName, playerColor, isLookingRight, isLobbyHost);

            playersManager.clients[playerId].FinishInitialization(serverPlayer);

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                // Connect existent players with the new client (including himself)
                packetsSender.SendPlayerConnectedPacket(client.playerId, playersManager.clients[playerId].serverPlayer);

                // Connect new player with each client (himself is already spawned)
                if (client.playerId != playerId)
                {
                    packetsSender.SendPlayerConnectedPacket(playerId, client.serverPlayer);
                }
            }
        }

        public void SavePlayerInput(int playerId, PlayerInput playerInput)
        {
            playersManager.clients[playerId].serverPlayer.remoteControllable.EnqueueInput(playerInput);
        }

        public void ChangePlayerColor(int playerId)
        {
            Interactable interactable = playersManager.clients[playerId].serverPlayer.nearbyInteractableChooser.chosen;
            if (interactable == null || interactable.GetType() != typeof(LobbyComputer))
            {
                Logger.LogError(SharedLoggerSection.PlayerColors, $"Attempt to change a color for player {playerId} when not being nearby a lobby computer");
                return;
            }

            interactable.Interact(playerId);
        }

        public void ScheduleGameStart()
        {
            if (playersManager.clients.Count < GameConfiguration.MinRequiredPlayersAmountForGame)
            {
                Logger.LogError(SharedLoggerSection.GameStart, "Attempt to start a game with too few players in it");
                return;
            }

            if (gameStartRequested)
            {
                Logger.LogError(SharedLoggerSection.GameStart, "Attempt to start a game which is already starting");
                return;
            }

            gameStartRequested = true;
            packetsSender.SendGameStartsPacket();
            metaMonoBehaviours.coroutines.StartCoroutine(StartGame());

            Logger.LogEvent(SharedLoggerSection.GameStart, "Game starts");
        }

        private IEnumerator StartGame()
        {
            yield return new WaitForSeconds(SecondsForGameLaunch);

            int[] impostorPlayerIds = GetImpostorPlayerIds();
            foreach (int impostorPlayerId in impostorPlayerIds)
            {
                playersManager.clients[impostorPlayerId].basePlayer.impostorable.isImpostor = true;
            }

            packetsSender.SendGameStartedPacket(impostorPlayerIds);

            scenesManager.LoadScene(Scene.Skeld);

            Logger.LogEvent(SharedLoggerSection.GameStart, $"Game has started. Impostors: {string.Join(", ", impostorPlayerIds)}");
        }

        private int[] GetImpostorPlayerIds()
        {
            int impostorsAmount = ComputePreferredImpostorsAmount();

            Random random = new Random();
            int[] playerIds = playersManager.clients.Keys.ToArray();
            int[] shuffledPlayerIds = playerIds.OrderBy(playerId => random.Next()).ToArray();

            return shuffledPlayerIds.Take(impostorsAmount).ToArray();
        }

        private int ComputePreferredImpostorsAmount()
        {
            if (playersManager.clients.Count >= 9)
            {
                return 3;
            }

            if (playersManager.clients.Count >= 6)
            {
                return 2;
            }

            return 1;
        }
    }
}
