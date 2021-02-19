using System.Collections;
using System.Linq;
using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Game;
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
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] public Lobby lobby;

        public const int SecondsForGameLaunch = GameConfiguration.SecondsForGameLaunch;

        public bool GameStarts { get; private set; }

        public void Initialize()
        {
            lobby = FindObjectOfType<Lobby>();

            GameStarts = false;
        }

        public bool IsActive()
        {
            return lobby != null;
        }

        public void ConnectPlayer(int playerId, string playerName)
        {
            Vector2 playerPosition = lobby.playerSpawnPrototypes[playerId].transform.position;
            ServerPlayer serverPlayer = Instantiate(playerPrefab, playerPosition, Quaternion.identity).GetComponent<ServerPlayer>();
            serverPlayer.gameObject.name = $"Player{playerId}";
            serverPlayer.transform.parent = lobby.playersContainer.transform;

            PlayerColor playerColor = PlayerColors.TakeFreeColor(playerId);
            bool isLookingRight = !lobby.playerSpawnPrototypes[playerId].spriteRenderer.flipX;
            bool isLobbyHost = PlayersManager.IsLobbyHost(playerId);
            serverPlayer.Initialize(playerId, playerName, playerColor, isLookingRight, isLobbyHost);

            playersManager.clients[playerId].FinishInitialization(serverPlayer);
            playersManager.basePlayersManager.players[playerId] = serverPlayer.basePlayer;
        }

        public void TryToScheduleGameStart(int calledPlayerId)
        {
            if (!PlayersManager.IsLobbyHost(calledPlayerId))
            {
                Logger.LogNotice(SharedLoggerSection.GameStart, $"Player {calledPlayerId} tries to schedule game start, but he's not the host");
                return;
            }

            if (playersManager.clients.Count < GameConfiguration.MinRequiredPlayersAmountForGame)
            {
                Logger.LogError(SharedLoggerSection.GameStart, "Attempt to start a game with too few players in it");
                return;
            }

            if (GameStarts)
            {
                Logger.LogError(SharedLoggerSection.GameStart, "Attempt to start a game which is already starting");
                return;
            }

            GameStarts = true;
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
