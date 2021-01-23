using System.Collections;
using System.Linq;
using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared;
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
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private Lobby lobby;

        public const int SecondsForGameLaunch = GameConfiguration.SecondsForGameLaunch;

        public void Initialize()
        {
            lobby = FindObjectOfType<Lobby>();
        }

        public void ConnectPlayer(int playerId, string playerName)
        {
            // Dummy with appropriate position, spriteRenderer.flipX from scene
            Player lobbyPlayerDummy = lobby.playerDummies[playerId];

            Vector2 playerPosition = lobbyPlayerDummy.transform.position;
            Player player = Instantiate(playerPrefab, playerPosition, Quaternion.identity).GetComponent<Player>();
            player.spriteRenderer.flipX = lobbyPlayerDummy.spriteRenderer.flipX;

            PlayersContainer playersContainer = FindObjectOfType<PlayersContainer>();
            if (playersContainer == null)
            {
                Logger.LogError(LoggerSection.Initialization, "Unable to find PlayersContainer object");
            }
            else
            {
                playersContainer.PlacePlayerIntoPlayersContainer(player.gameObject);
            }

            PlayerColor playerColor = PlayerColors.TakeFreeColor(playerId);
            bool isLobbyHost = playerId == PlayersManager.MinPlayerId;
            player.Initialize(playerId, playerName, playerColor, isLobbyHost);

            playersManager.clients[playerId].FinishInitialization(player);

            foreach (Client client in playersManager.clients.Values)
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                // Connect existent players with the new client (including himself)
                packetsSender.SendPlayerConnectedPacket(client.playerId, playersManager.clients[playerId].player);

                // Connect new player with each client (himself is already spawned)
                if (client.playerId != playerId)
                {
                    packetsSender.SendPlayerConnectedPacket(playerId, client.player);
                }
            }
        }

        public void SavePlayerInput(int playerId, PlayerInput playerInput)
        {
            playersManager.clients[playerId].player.remoteControllable.EnqueueInput(playerInput);
        }

        public void ChangePlayerColor(int playerId)
        {
            PlayerColor newPlayerColor = PlayerColors.SwitchToRandomColor(playerId);

            playersManager.clients[playerId].player.colorable.ChangeColor(newPlayerColor);
            packetsSender.SendColorChanged(playerId, newPlayerColor);

            Logger.LogEvent(SharedLoggerSection.PlayerColors, $"Changed player {playerId} color to {Helpers.GetEnumName(newPlayerColor)}");
        }

        public void ScheduleGameStart()
        {
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
                playersManager.clients[impostorPlayerId].player.information.isImposter = true;
            }

            packetsSender.SendGameStartedPacket(impostorPlayerIds);

            ScenesManager.LoadScene(Scene.Skeld);

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
