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

        public void ConnectPlayer(int playerId, string playerName)
        {
            Player player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();

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

        public void DisconnectPlayer(int playerId)
        {
            if (!playersManager.clients.ContainsKey(playerId))
            {
                Logger.LogNotice(LoggerSection.Connection, $"Skipping player {playerId} disconnect, because it is already disconnected");
                return;
            }

            Logger.LogEvent(LoggerSection.Connection, $"{playersManager.clients[playerId].GetTcpEndPoint()} has disconnected (player {playerId})");

            if (playersManager.clients[playerId].player.information.isLobbyHost)
            {
                packetsSender.SendKickedPacket(playerId);
                foreach (int playerIdToRemove in playersManager.clients.Keys.ToList())
                {
                    RemovePlayerFromGame(playerIdToRemove);
                }

                Logger.LogEvent(LoggerSection.Connection, $"Removed every player, because the host player {playerId} has disconnected");
            }
            else
            {
                packetsSender.SendPlayerDisconnectedPacket(playerId);
                RemovePlayerFromGame(playerId);
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
            yield return new WaitForSeconds(PlayersManager.SecondsForGameLaunch);

            int[] impostorPlayerIds = GetImpostorPlayerIds();
            foreach (int impostorPlayerId in impostorPlayerIds)
            {
                playersManager.clients[impostorPlayerId].player.information.isImposter = true;
            }

            packetsSender.SendGameStartedPacket(impostorPlayerIds);

            Logger.LogEvent(SharedLoggerSection.GameStart, $"Game has started. Impostors: {string.Join(", ", impostorPlayerIds)}");
        }

        private void RemovePlayerFromGame(int playerId)
        {
            PlayerColors.ReleasePlayerColor(playerId);
            Destroy(playersManager.clients[playerId].player.gameObject);
            playersManager.clients.Remove(playerId);
        }

        private int[] GetImpostorPlayerIds()
        {
            int impostorsAmount = playersManager.clients.Count > 7 ? 2 : 1;

            Random random = new Random();
            int[] playerIds = playersManager.clients.Keys.ToArray();
            int[] shuffledPlayerIds = playerIds.OrderBy(playerId => random.Next()).ToArray();

            return shuffledPlayerIds.Take(impostorsAmount).ToArray();
        }
    }
}
