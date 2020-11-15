using System;
using System.Collections.Generic;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;

namespace AmongUsClone.Server.Networking
{
    public static class PacketsReceiver
    {
        private static readonly Dictionary<int, TcpConnection.OnPacketReceivedCallback> PacketHandlers = new Dictionary<int, TcpConnection.OnPacketReceivedCallback>
        {
            {(int) ClientPacketType.WelcomeReceived, ProcessWelcomeReceivedPacket},
            {(int) ClientPacketType.UdpTestReceived, ProcessUdpTestReceivedPacket}
        };

        public static void ProcessPacket(int clientId, int packetTypeId, Packet packet)
        {
            PacketHandlers[packetTypeId](clientId, packet);
        }

        private static void ProcessWelcomeReceivedPacket(int clientId, Packet packet)
        {
            int packetClientId = packet.ReadInt();
            string userName = packet.ReadString();

            if (clientId != packetClientId)
            {
                throw new Exception($"Bad clientId {clientId} received");
            }

            string packetTypeName = GetPacketTypeName(ClientPacketType.WelcomeReceived);
            Logger.LogEvent($"Received «{packetTypeName}» packet from the client {clientId}");
            
            // Todo: send the player into the game
            
            Logger.LogEvent($"{Server.Clients[clientId].TcpConnectionToClient.TcpClient.Client.RemoteEndPoint} connected successfully and is now a player {clientId}");
        }

        private static void ProcessUdpTestReceivedPacket(int clientId, Packet packet)
        {
            string message = packet.ReadString();
            
            Logger.LogEvent($"Received a packet via UDP. Contains message: {message}");
        }

        private static string GetPacketTypeName(ClientPacketType clientPacketType)
        {
            return Enum.GetName(typeof(ClientPacketType), clientPacketType);
        }
    }
}
