// I want a class to have either static or non static methods
// ReSharper disable MemberCanBeMadeStatic.Global

using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;
using GameObject = UnityEngine.GameObject;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game
{
    // Todo: migrate all singletons to Scriptable Objects
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance; // Instance is needed in order to have inspector variables and call MonoBehaviour methods

        public Lobby lobby;

        public GameObject serverMovablePrefab;

        private void Awake()
        {
            if (instance != null)
            {
                Logger.LogCritical(LoggerSection.Initialization, "Attempt to instantiate GameLogic second time");
            }

            instance = this;
        }

        public void ConnectPlayer(int playerId, string playerName)
        {
            Player player = lobby.AddPlayer(Vector3.zero, serverMovablePrefab).GetComponent<Player>();

            PlayerColor playerColor = PlayerColors.TakeFreeColor(playerId);
            player.Initialize(playerId, playerName, playerColor);
            Server.clients[playerId].FinishInitialization(player);

            foreach (Client client in Server.clients.Values)
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                // Connect existent players with the new client (including himself)
                PacketsSender.SendPlayerConnectedPacket(client.playerId, Server.clients[playerId].player);

                // Connect new player with each client (himself is already spawned)
                if (client.playerId != playerId)
                {
                    PacketsSender.SendPlayerConnectedPacket(playerId, client.player);
                }
            }
        }

        public void DisconnectPlayer(int playerId)
        {
            Logger.LogEvent(LoggerSection.Connection, $"{Server.clients[playerId].GetTcpEndPoint()} has disconnected (player {playerId})");

            PlayerColors.ReleasePlayerColor(playerId);
            Destroy(Server.clients[playerId].player.gameObject);
            Server.clients.Remove(playerId);
            PacketsSender.SendPlayerDisconnectedPacket(playerId);
        }

        public void SavePlayerInput(int playerId, PlayerInput playerInput)
        {
            Server.clients[playerId].player.remoteControllable.EnqueueInput(playerInput);
        }
    }
}
