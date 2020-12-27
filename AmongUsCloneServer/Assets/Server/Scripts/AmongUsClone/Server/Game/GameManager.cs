// I want a class to have either static or non static methods
// ReSharper disable MemberCanBeMadeStatic.Global

using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Server.Snapshots;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game
{
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
            Server.clients[playerId].player = lobby.AddPlayer(playerId, playerName, Vector2.zero, serverMovablePrefab);
            ProcessedPlayerInputs.Initialize(playerId);

            foreach (Client client in Server.clients.Values)
            {
                if (!client.IsPlayerInitialized())
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

            lobby.RemovePlayer(playerId);
            Server.clients.Remove(playerId);
            PacketsSender.SendPlayerDisconnectedPacket(playerId);
        }

        public void SavePlayerInput(int playerId, int inputId, PlayerInput playerInput)
        {
            ProcessedPlayerInputs.Update(playerId, inputId); // Todo: move into actual usage
            Server.clients[playerId].player.GetComponent<ServerPlayer>().EnqueueInput(playerInput);
        }
    }
}
