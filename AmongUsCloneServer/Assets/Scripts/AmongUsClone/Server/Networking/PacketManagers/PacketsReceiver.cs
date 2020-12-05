using System;
using System.Collections.Generic;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;

namespace AmongUsClone.Server.Networking.PacketManagers
{
    public static class PacketsReceiver
    {
        private static readonly Dictionary<int, TcpConnection.OnPacketReceivedCallback> packetHandlers = new Dictionary<int, TcpConnection.OnPacketReceivedCallback>
        {
            {(int) ClientPacketType.WelcomeReceived, ProcessWelcomeReceivedPacket},
            {(int) ClientPacketType.PlayerInput, ProcessPlayerInputPacket},
        };

        public static void ProcessPacket(int playerId, int packetTypeId, Packet packet, bool isTcp)
        {
            string packetTypeName = GetPacketTypeName((ClientPacketType)packetTypeId);
            string protocolName = isTcp ? "TCP" : "UDP";
            Logger.LogEvent(LoggerSection.Network, $"Received «{packetTypeName}» {protocolName} packet from the client {playerId}");

            packetHandlers[packetTypeId](playerId, packet);
        }

        private static void ProcessWelcomeReceivedPacket(int playerId, Packet packet)
        {
            int packetPlayerId = packet.ReadInt();
            string userName = packet.ReadString();

            if (playerId != packetPlayerId)
            {
                throw new Exception($"Bad playerId {playerId} received");
            }

            Logger.LogEvent(LoggerSection.ClientConnection, $"{Server.clients[playerId].GetTcpEndPoint()} connected successfully and is now a player {playerId}");

            Game.instance.ConnectPlayer(playerId, userName);
        }

        private static void ProcessPlayerInputPacket(int playerId, Packet packet)
        {
            int playerInputsAmount = packet.ReadInt();

            // Because of multi threading we might not have this client
            if (!Server.clients.ContainsKey(playerId))
            {
                return;
            }

            bool[] serializedPlayerInput = new bool[playerInputsAmount];
            for (int playerInputIndex = 0; playerInputIndex < serializedPlayerInput.Length; playerInputIndex++)
            {
                serializedPlayerInput[playerInputIndex] = packet.ReadBool();
            }

            PlayerInput playerInput = PlayerInput.Deserialize(serializedPlayerInput);
            Game.instance.UpdatePlayerInput(playerId, playerInput);
        }

        private static string GetPacketTypeName(ClientPacketType clientPacketType)
        {
            return Enum.GetName(typeof(ClientPacketType), clientPacketType);
        }
    }
}
