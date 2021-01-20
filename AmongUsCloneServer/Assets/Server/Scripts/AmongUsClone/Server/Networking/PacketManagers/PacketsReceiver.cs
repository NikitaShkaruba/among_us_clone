using System;
using System.Collections.Generic;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Networking.PacketManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PacketsReceiver", menuName = "PacketsReceiver")]
    public class PacketsReceiver : ScriptableObject
    {
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;

        private Dictionary<int, TcpConnection.OnPacketReceivedCallback> packetHandlers;

        public void OnEnable()
        {
            packetHandlers = new Dictionary<int, TcpConnection.OnPacketReceivedCallback>
            {
                {(int) ClientPacketType.WelcomeReceived, ProcessWelcomeReceivedPacket},
                {(int) ClientPacketType.PlayerInput, ProcessPlayerInputPacket},
                {(int) ClientPacketType.ColorChangeRequest, ProcessColorChangeRequestPacket},
                {(int) ClientPacketType.StartGame, ProcessStartGamePacket}
            };
        }

        public void ProcessPacket(int playerId, int packetTypeId, Packet packet, bool isTcp)
        {
            string packetTypeName = Helpers.GetEnumName((ClientPacketType) packetTypeId);
            string protocolName = isTcp ? "TCP" : "UDP";
            Logger.LogEvent(LoggerSection.Network, $"Received «{packetTypeName}» {protocolName} packet from the client {playerId}");

            packetHandlers[packetTypeId](playerId, packet);
        }

        private void ProcessWelcomeReceivedPacket(int playerId, Packet packet)
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

            Logger.LogEvent(LoggerSection.Connection, $"{playersManager.clients[playerId].GetTcpEndPoint()} connected successfully and is now a player {playerId}");

            lobbyGamePhase.ConnectPlayer(playerId, userName);
        }

        private void ProcessPlayerInputPacket(int playerId, Packet packet)
        {
            // Because of multi threading we might not have this client yet
            if (!playersManager.clients.ContainsKey(playerId))
            {
                return;
            }

            PlayerInput playerInput = packet.ReadPlayerInput();

            lobbyGamePhase.SavePlayerInput(playerId, playerInput);
        }

        private void ProcessColorChangeRequestPacket(int playerId, Packet packet)
        {
            lobbyGamePhase.ChangePlayerColor(playerId);
        }

        private void ProcessStartGamePacket(int playerId, Packet packet)
        {
            if (playerId != PlayersManager.MinPlayerId)
            {
                Logger.LogNotice(SharedLoggerSection.GameStart, "Not host trying to start the game");
                return;
            }

            lobbyGamePhase.ScheduleGameStart();
        }
    }
}
