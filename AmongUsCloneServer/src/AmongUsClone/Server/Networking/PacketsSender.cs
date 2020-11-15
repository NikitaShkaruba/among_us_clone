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

        public static void SendUdpTestPacket(int clientId)
        {
            const ServerPacketType serverPacketType = ServerPacketType.UdpTest;

            using (Packet packet = new Packet((int) serverPacketType))
            {
                packet.Write("A test UDP packet");
                SendUdpPacket(clientId, serverPacketType, packet);
            }
        }

        private static void SendTcpPacket(int clientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            Server.Clients[clientId].TcpConnectionToClient.SendPacket(packet);

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent «{packetTypeName}» TCP packet to the client {clientId}");
        }

        private static void SendTcpPacketToAll(ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            for (int clientId = Server.MinPlayerId; clientId <= Server.MaxPlayerId; clientId++)
            {
                Server.Clients[clientId].TcpConnectionToClient.SendPacket(packet);
            }
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent «{packetTypeName}» TCP packet to all clients");
        }
 
        private static void SendTcpPacketToAllExceptOne(int exceptClientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            for (int clientId = Server.MinPlayerId; clientId <= Server.MaxPlayerId; clientId++)
            {
                if (clientId == exceptClientId)
                {
                    continue;
                }
                
                Server.Clients[clientId].TcpConnectionToClient.SendPacket(packet);
            }

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent «{packetTypeName}» TCP packet to all clients except ${exceptClientId}");
        }

        private static void SendUdpPacket(int clientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[clientId].UdpConnectionToClient.SendPacket(packet);
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent «{packetTypeName}» UDP packet to the client {clientId}");
        }

        private static void SendUdpPacketToAll(ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            for (int clientId = Server.MinPlayerId; clientId <= Server.MaxPlayerId; clientId++)
            {
                Server.Clients[clientId].UdpConnectionToClient.SendPacket(packet);
            }
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent «{packetTypeName}» UDP packet to all clients");
        }
 
        private static void SendUdpPacketToAllExceptOne(int exceptClientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            for (int clientId = Server.MinPlayerId; clientId <= Server.MaxPlayerId; clientId++)
            {
                if (clientId == exceptClientId)
                {
                    continue;
                }
                
                Server.Clients[clientId].UdpConnectionToClient.SendPacket(packet);
            }

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent «{packetTypeName}» UDP packet to all clients except ${exceptClientId}");
        }

        private static string GetPacketTypeName(ServerPacketType serverPacketType)
        {
            return Enum.GetName(typeof(ServerPacketType), serverPacketType);
        }
    }
}
