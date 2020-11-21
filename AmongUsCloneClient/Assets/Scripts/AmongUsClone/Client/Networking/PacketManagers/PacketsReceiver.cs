using System.Collections.Generic;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;
using Vector2 = AmongUsClone.Shared.DataStructures.Vector2;

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
            {(int) ServerPacketType.PlayerPosition, ProcessPlayerPositionPacket},
        };

        public static void ProcessPacket(int packetTypeId, Packet packet)
        {
            packetHandlers[packetTypeId](packet);
        }

        private static void ProcessWelcomePacket(Packet packet)
        {
            int myPlayerId = packet.ReadInt();

            Game.instance.connectionToServer.FinishConnection(myPlayerId);
        }

        private static void ProcessPlayerConnectedPacket(Packet packet)
        {
            int playerId = packet.ReadInt();
            string playerName = packet.ReadString();
            Vector2 playerPosition = packet.ReadVector2();

            Game.instance.AddPlayerToLobby(playerId, playerName, playerPosition);
        }

        private static void ProcessPlayerDisconnectedPacket(Packet packet)
        {
            int playerId = packet.ReadInt();

            Game.instance.RemovePlayerFromLobby(playerId);
        }

        private static void ProcessPlayerPositionPacket(Packet packet)
        {
            int playerId = packet.ReadInt();
            Vector2 playerPosition = packet.ReadVector2();

            Game.instance.UpdatePlayerPosition(playerId, playerPosition);
        }
    }
}
