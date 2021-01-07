using System;
using System.Collections.Generic;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
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
            {(int) ClientPacketType.ColorChangeRequest, ProcessColorChangeRequestPacket}
        };

        public static void ProcessPacket(int playerId, int packetTypeId, Packet packet, bool isTcp)
        {
            string packetTypeName = Helpers.GetEnumName((ClientPacketType) packetTypeId);
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

            if (userName.Equals(""))
            {
                Logger.LogError(LoggerSection.Connection, $"Bad user name passed from player {playerId}");
                return;
            }

            Logger.LogEvent(LoggerSection.Connection, $"{Server.clients[playerId].GetTcpEndPoint()} connected successfully and is now a player {playerId}");

            GameManager.instance.ConnectPlayer(playerId, userName);
        }

        private static void ProcessPlayerInputPacket(int playerId, Packet packet)
        {
            // Because of multi threading we might not have this client yet
            if (!Server.clients.ContainsKey(playerId))
            {
                return;
            }

            PlayerInput playerInput = packet.ReadPlayerInput();

            GameManager.instance.SavePlayerInput(playerId, playerInput);
        }

        private static void ProcessColorChangeRequestPacket(int playerId, Packet packet)
        {
            GameManager.instance.ChangePlayerColor(playerId);
        }
    }
}
