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

        public static void SendSpawnPlayerPacket(int clientId, Player player)
        {
            const ServerPacketType packetType = ServerPacketType.SpawnPlayer;

            using (Packet packet = new Packet((int) packetType))
            {
                packet.Write(player.Id);
                packet.Write(player.Name);
                packet.Write(player.Position);
                
                SendTcpPacket(clientId, packetType, packet);
            }
        }
 
        public static void SendPositionPacket(Player player)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerPosition;

            using (Packet packet = new Packet((int) packetType))
            {
                packet.Write(player.Id);
                packet.Write(player.Position);
                
                SendUdpPacketToAll(packetType, packet);
            }
        }

        private static void SendTcpPacket(int clientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            Server.Clients[clientId].TcpConnectionToClient.SendPacket(packet);

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» TCP packet to the client {clientId}");
        }

        private static void SendTcpPacketToAll(ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            foreach (Client client in Server.Clients.Values)
            {
                client.TcpConnectionToClient.SendPacket(packet);
            }
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» TCP packet to all clients");
        }
 
        private static void SendTcpPacketToAllExceptOne(int exceptClientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            foreach (Client client in Server.Clients.Values)
            {
                if (client.Id == exceptClientId)
                {
                    continue;
                }
                
                client.TcpConnectionToClient.SendPacket(packet);
            }

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» TCP packet to all clients except ${exceptClientId}");
        }

        private static void SendUdpPacket(int clientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[clientId].UdpConnectionToClient.SendPacket(packet);
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» UDP packet to the client {clientId}");
        }

        private static void SendUdpPacketToAll(ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.Clients.Values)
            {
                client.UdpConnectionToClient.SendPacket(packet);
            }
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent(LoggerSection.Network, $"Sent «{packetTypeName}» UDP packet to all clients");
        }

        private static void SendUdpPacketToAllExceptOne(int exceptClientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            foreach (Client client in Server.Clients.Values)
            {
                if (client.Id == exceptClientId)
                {
                    continue;
                }
                
                client.UdpConnectionToClient.SendPacket(packet);
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
