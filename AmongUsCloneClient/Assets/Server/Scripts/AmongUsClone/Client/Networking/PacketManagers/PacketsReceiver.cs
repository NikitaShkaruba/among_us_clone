using System.Collections.Generic;
using AmongUsClone.Client.Game;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking.PacketManagers
{
    public class PacketsReceiver : MonoBehaviour
    {
        private delegate void OnPacketReceivedCallback(Packet packet);

        private static readonly Dictionary<int, OnPacketReceivedCallback> packetHandlers = new Dictionary<int, OnPacketReceivedCallback>
        {
            {(int) ServerPacketType.Welcome, ProcessWelcomePacket},
            {(int) ServerPacketType.PlayerConnected, ProcessPlayerConnectedPacket},
            {(int) ServerPacketType.PlayerDisconnected, ProcessPlayerDisconnectedPacket},
            {(int) ServerPacketType.GameSnapshot, ProcessGameSnapshotPacket},
        };

        public static void ProcessPacket(int packetTypeId, Packet packet, bool isTcp)
        {
            string protocolName = isTcp ? "TCP" : "UDP";
            Logger.LogEvent(LoggerSection.Network, $"Received «{Helpers.GetEnumName((ServerPacketType)packetTypeId)}» {protocolName} packet from server");

            packetHandlers[packetTypeId](packet);
        }

        private static void ProcessWelcomePacket(Packet packet)
        {
            int myPlayerId = packet.ReadInt();

            GameManager.instance.connectionToServer.FinishConnection(myPlayerId);

            Logger.LogEvent(LoggerSection.Connection, $"Connected successfully to server. My player id is {myPlayerId}");
        }

        private static void ProcessPlayerConnectedPacket(Packet packet)
        {
            int playerId = packet.ReadInt();
            string playerName = packet.ReadString();
            Vector2 playerPosition = packet.ReadVector2();

            GameManager.instance.AddPlayerToLobby(playerId, playerName, playerPosition);

            Logger.LogEvent(LoggerSection.Connection, $"Added player {playerId} to lobby");
        }

        private static void ProcessPlayerDisconnectedPacket(Packet packet)
        {
            int playerId = packet.ReadInt();

            GameManager.instance.RemovePlayerFromLobby(playerId);

            Logger.LogEvent(LoggerSection.Connection, $"Player {playerId} has disconnected");
        }

        private static void ProcessGameSnapshotPacket(Packet packet)
        {
            int snapshotId = packet.ReadInt();
            int snapshotPlayersAmount = packet.ReadInt();

            for (int snapshotPlayerIndex = 0; snapshotPlayerIndex < snapshotPlayersAmount; snapshotPlayerIndex++)
            {
                int playerId = packet.ReadInt();
                Vector2 playerPosition = packet.ReadVector2();

                GameManager.instance.UpdatePlayerPosition(playerId, playerPosition);
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {snapshotId}");
        }
    }
}
