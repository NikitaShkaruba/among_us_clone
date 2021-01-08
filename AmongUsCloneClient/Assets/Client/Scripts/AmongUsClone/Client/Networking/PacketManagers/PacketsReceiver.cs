using System;
using System.Collections.Generic;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Snapshots;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using Helpers = AmongUsClone.Shared.Helpers;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking.PacketManagers
{
    public class PacketsReceiver : MonoBehaviour
    {
        private delegate void OnPacketReceivedCallback(Packet packet);

        private static readonly Dictionary<int, OnPacketReceivedCallback> packetHandlers = new Dictionary<int, OnPacketReceivedCallback>
        {
            {(int) ServerPacketType.Welcome, ProcessWelcomePacket},
            {(int) ServerPacketType.Kicked, ProcessKickedPacket},
            {(int) ServerPacketType.PlayerConnected, ProcessPlayerConnectedPacket},
            {(int) ServerPacketType.PlayerDisconnected, ProcessPlayerDisconnectedPacket},
            {(int) ServerPacketType.GameSnapshot, ProcessGameSnapshotPacket},
            {(int) ServerPacketType.ColorChanged, ProcessPlayerColorChangedPacket}
        };

        public static void ProcessPacket(int packetTypeId, Packet packet, bool isTcp)
        {
            string protocolName = isTcp ? "TCP" : "UDP";
            Logger.LogEvent(LoggerSection.Network, $"Received «{Helpers.GetEnumName((ServerPacketType) packetTypeId)}» {protocolName} packet from server");

            packetHandlers[packetTypeId](packet);
        }

        private static void ProcessWelcomePacket(Packet packet)
        {
            Action action = () =>
            {
                int myPlayerId = packet.ReadInt();
                int maxPlayersAmount = packet.ReadInt();
                int minRequiredPlayersAmountForGame = packet.ReadInt();

                GameManager.instance.connectionToServer.FinishConnection(myPlayerId);
                GameManager.instance.InitializeGameSettings(maxPlayersAmount, minRequiredPlayersAmountForGame);

                Logger.LogEvent(LoggerSection.Connection, $"Connected successfully to server. My player id is {myPlayerId}");
            };

            NetworkSimulation.instance.ReceiveThroughNetwork(action);
        }

        private static void ProcessKickedPacket(Packet packet)
        {
            Action action = () =>
            {
                GameManager.instance.DisconnectFromLobby();
                Logger.LogEvent(LoggerSection.Connection, "Received a kick from server");
            };

            NetworkSimulation.instance.ReceiveThroughNetwork(action);
        }

        private static void ProcessPlayerConnectedPacket(Packet packet)
        {
            Action action = () =>
            {
                int playerId = packet.ReadInt();
                string playerName = packet.ReadString();
                bool isPlayerLobbyHost = packet.ReadBool();
                PlayerColor playerColor = (PlayerColor) packet.ReadInt();
                Vector2 playerPosition = packet.ReadVector2();

                GameManager.instance.AddPlayerToLobby(playerId, playerName, playerColor, playerPosition, isPlayerLobbyHost);

                Logger.LogEvent(LoggerSection.Connection, $"Added player {playerId} to lobby");
            };

            NetworkSimulation.instance.ReceiveThroughNetwork(action);
        }

        private static void ProcessPlayerDisconnectedPacket(Packet packet)
        {
            Action action = () =>
            {
                int playerId = packet.ReadInt();

                GameManager.instance.RemovePlayerFromLobby(playerId);

                Logger.LogEvent(LoggerSection.Connection, $"Player {playerId} has disconnected");
            };

            NetworkSimulation.instance.ReceiveThroughNetwork(action);
        }

        private static void ProcessGameSnapshotPacket(Packet packet)
        {
            Action action = () =>
            {
                ClientGameSnapshot gameSnapshot = packet.ReadClientGameSnapshot();
                GameSnapshots.ProcessSnapshot(gameSnapshot);
            };

            NetworkSimulation.instance.ReceiveThroughNetwork(action);
        }

        private static void ProcessPlayerColorChangedPacket(Packet packet)
        {
            Action action = () =>
            {
                int playerId = packet.ReadInt();
                PlayerColor playerColor = (PlayerColor) packet.ReadInt();

                GameManager.instance.ChangePlayerColor(playerId, playerColor);
            };

            NetworkSimulation.instance.ReceiveThroughNetwork(action);
        }
    }
}
