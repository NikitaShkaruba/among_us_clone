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
            {(int) ClientPacketType.PlayerInput, ProcessPlayerInputPacket},
        };

        public static void ProcessPacket(int clientId, int packetTypeId, Packet packet, bool isTcp)
        {
            string packetTypeName = GetPacketTypeName((ClientPacketType)packetTypeId);
            string protocolName = isTcp ? "TCP" : "UDP";
            Logger.LogEvent(LoggerSection.Network, $"Received «{packetTypeName}» {protocolName} packet from the client {clientId}");

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

            Logger.LogEvent(LoggerSection.ClientConnection, $"{Server.Clients[clientId].TcpConnectionToClient.TcpClient.Client.RemoteEndPoint} connected successfully and is now a player {clientId}");
            
            Server.Clients[clientId].SendIntoGame(userName);
        }

        private static void ProcessPlayerInputPacket(int clientId, Packet packet)
        {
            int playerInputsAmount = packet.ReadInt();
            
            bool[] playerInputs = new bool[playerInputsAmount];
            for (int playerInputIndex = 0; playerInputIndex < playerInputs.Length; playerInputIndex++)
            {
                playerInputs[playerInputIndex] = packet.ReadBool();
            }

            Server.Clients[clientId].Player.SetInputs(playerInputs);
        }

        private static string GetPacketTypeName(ClientPacketType clientPacketType)
        {
            return Enum.GetName(typeof(ClientPacketType), clientPacketType);
        }
    }
}
