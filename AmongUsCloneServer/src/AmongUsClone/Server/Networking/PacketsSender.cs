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

                SendPacket(clientId, packetTypeId, packet);
            }
        }

        private static void SendPacket(int clientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            Server.Clients[clientId].TcpConnection.SendPacket(packet);

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent data packet «{packetTypeName}» to the client {clientId}");
        }

        private static void SendPacketToAll(ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            for (int clientId = Server.MinPlayerId; clientId <= Server.MaxPlayerId; clientId++)
            {
                Server.Clients[clientId].TcpConnection.SendPacket(packet);
            }
            
            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent data packet «{packetTypeName}» to all clients");
        }
 
        private static void SendPacketToAllExceptOne(int exceptClientId, ServerPacketType packetTypeId, Packet packet)
        {
            packet.WriteLength();
            
            for (int clientId = Server.MinPlayerId; clientId <= Server.MaxPlayerId; clientId++)
            {
                if (clientId == exceptClientId)
                {
                    continue;
                }
                
                Server.Clients[clientId].TcpConnection.SendPacket(packet);
            }

            string packetTypeName = GetPacketTypeName(packetTypeId);
            Logger.LogEvent($"Sent data packet «{packetTypeName}» to all clients except ${exceptClientId}");
        }

        private static string GetPacketTypeName(ServerPacketType serverPacketType)
        {
            return Enum.GetName(typeof(ServerPacketType), serverPacketType);
        }
    }
}
