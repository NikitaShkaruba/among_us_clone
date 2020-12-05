// I want a class to have either static or non static methods
// ReSharper disable MemberCanBeMadeStatic.Global

using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared;
using UnityEngine;
using Logger = AmongUsClone.Server.Infrastructure.Logger;

namespace AmongUsClone.Server
{
    public class Game : MonoBehaviour
    {
        public static Game instance; // Instance is needed in order to have inspector variables and call MonoBehaviour methods

        public GameObject playerPrefab;

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
            Server.clients[playerId].player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).GetComponent<Player>();
            Server.clients[playerId].player.Initialize(playerId, playerName);

            foreach (Client client in Server.clients.Values)
            {
                // Because of a multithreading we should check for it
                if (client.player == null)
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
            Logger.LogEvent(LoggerSection.ClientConnection, $"{Server.clients[playerId].GetTcpEndPoint()} has disconnected (player {playerId})");

            Destroy(Server.clients[playerId].player.gameObject);
            Server.clients.Remove(playerId);
            PacketsSender.SendPlayerDisconnectedPacket(playerId);
        }

        public void UpdatePlayerInput(int playerId, PlayerInput playerInput)
        {
            Server.clients[playerId].player.UpdateInput(playerInput);
        }
    }
}
