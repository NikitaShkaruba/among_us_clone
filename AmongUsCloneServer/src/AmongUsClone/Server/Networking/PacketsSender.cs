using System;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;

namespace AmongUsClone.Server.Networking
{
    public static class PacketsSender
    {
        public static void SendWelcomePacket(int clientId, string message)
        {
            const ServerPacketType packetTypeId = ServerPacketType.Welcome;

            using (Packet packet = new Packet((int)packetTypeId))
            {
                packet.Write(message);
                packet.Write(clientId);

                SendTcpPacket(clientId, packetTypeId, packet);
            }
        }

        public static void SendPlayerConnectedPacket(int clientId, Player connectedPlayer)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerConnected;

            using (Packet packet = new Packet((int) packetType))
            {
                packet.Write(connectedPlayer.id);
                packet.Write(connectedPlayer.name);
                packet.Write(connectedPlayer.position);
                
                SendTcpPacket(clientId, packetType, packet);
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

        public static void SendPositionPacket(Player player)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerPosition;

            using (Packet packet = new Packet((int) packetType))
            {
                packet.Write(player.id);
                packet.Write(player.position);
                
                SendUdpPacketToAll(packetType, packet);
            }
        }

        private static void SendTcpPacket(int clientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            Server.Clients[clientId].tcpConnectionToClient.SendPacket(packet);

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» TCP packet to the client {clientId}");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendTcpPacketToAll(ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            foreach (Client client in Server.Clients.Values)
            {
                client.tcpConnectionToClient.SendPacket(packet);
            }
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» TCP packet to all clients");
        }
 
        // ReSharper disable once UnusedMember.Local
        private static void SendTcpPacketToAllExceptOne(int exceptClientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            foreach (Client client in Server.Clients.Values)
            {
                if (client.id == exceptClientId)
                {
                    continue;
                }
                
                client.tcpConnectionToClient.SendPacket(packet);
            }

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» TCP packet to all clients except ${exceptClientId}");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendUdpPacket(int clientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[clientId].udpConnectionToClient.SendPacket(packet);
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» UDP packet to the client {clientId}");
        }

        private static void SendUdpPacketToAll(ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.Clients.Values)
            {
                client.udpConnectionToClient.SendPacket(packet);
            }
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» UDP packet to all clients");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendUdpPacketToAllExceptOne(int exceptClientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            foreach (Client client in Server.Clients.Values)
            {
                if (client.id == exceptClientId)
                {
                    continue;
                }
                
                client.udpConnectionToClient.SendPacket(packet);
            }

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» UDP packet to all clients except ${exceptClientId}");
        }

        private static string GetPacketTypeName(ServerPacketType serverPacketType)
        {
            return Enum.GetName(typeof(ServerPacketType), serverPacketType);
        }
    }
}
