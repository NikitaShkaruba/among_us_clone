using AmongUsClone.Server.Game;
using AmongUsClone.Server.Game.Snapshots;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;

namespace AmongUsClone.Server.Networking.PacketManagers
{
    public static class PacketsSender
    {
        public static void SendWelcomePacket(int playerId)
        {
            const ServerPacketType packetTypeId = ServerPacketType.Welcome;

            using (Packet packet = new Packet((int) packetTypeId))
            {
                packet.Write(playerId);

                SendTcpPacket(playerId, packetTypeId, packet);
            }
        }

        public static void SendPlayerConnectedPacket(int playerId, Player connectedPlayer)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerConnected;

            using (Packet packet = new Packet((int) packetType))
            {
                packet.Write(connectedPlayer.id);
                packet.Write(connectedPlayer.name);
                packet.Write(connectedPlayer.transform.position);

                SendTcpPacket(playerId, packetType, packet);
            }
        }

        public static void SendPlayerDisconnectedPacket(int playerId)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerDisconnected;

            using (Packet packet = new Packet((int) packetType))
            {
                packet.Write(playerId);

                SendTcpPacketToAll(packetType, packet);
            }
        }

        public static void SendGameSnapshotPackets(GameSnapshot gameSnapshot)
        {
            const ServerPacketType packetType = ServerPacketType.GameSnapshot;

            using (Packet packet = new Packet((int) packetType))
            {
                packet.Write(gameSnapshot.id);
                packet.Write(gameSnapshot.players.Count);

                foreach (Player player in gameSnapshot.players)
                {
                    packet.Write(player.id);
                    packet.Write(player.Position);
                }

                SendUdpPacketToAll(packetType, packet);
            }
        }

        private static void SendTcpPacket(int playerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            Server.clients[playerId].SendTcpPacket(packet);

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» TCP packet to the client {playerId}");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendTcpPacketToAll(ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.clients.Values)
            {
                client.SendTcpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» TCP packet to all clients");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendTcpPacketToAllExceptOne(int ignoredPlayerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.clients.Values)
            {
                if (client.playerId == ignoredPlayerId)
                {
                    continue;
                }

                client.SendTcpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» TCP packet to all clients except ${ignoredPlayerId}");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendUdpPacket(int playerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();
            Server.clients[playerId].SendUdpPacket(packet);

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» UDP packet to the client {playerId}");
        }

        private static void SendUdpPacketToAll(ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.clients.Values)
            {
                client.SendUdpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» UDP packet to all clients");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendUdpPacketToAllExceptOne(int ignoredPlayerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.clients.Values)
            {
                if (client.playerId == ignoredPlayerId)
                {
                    continue;
                }

                client.SendUdpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» UDP packet to all clients except ${ignoredPlayerId}");
        }
    }
}
