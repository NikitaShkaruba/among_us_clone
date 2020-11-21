using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared;
using AmongUsClone.Shared.DataStructures;

namespace AmongUsClone.Server
{
    public static class GameLogic
    {
        public static void ConnectPlayer(int playerId, string playerName)
        {
            Server.clients[playerId].player = new Player(playerId, playerName, new Vector2(0, 0));

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

        public static void DisconnectPlayer(int playerId)
        {
            Logger.LogEvent(LoggerSection.ClientConnection, $"{Server.clients[playerId].GetTcpEndPoint()} has disconnected (player {playerId})");

            Server.clients.Remove(playerId);
            PacketsSender.SendPlayerDisconnectedPacket(playerId);
        }

        public static void UpdatePlayerInput(int playerId, PlayerInput playerInput)
        {
            Server.clients[playerId].player.UpdateInput(playerInput);
        }
    }
}
