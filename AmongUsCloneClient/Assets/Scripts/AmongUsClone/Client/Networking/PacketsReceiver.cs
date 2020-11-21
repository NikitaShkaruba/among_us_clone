using System.Collections.Generic;
using System.Net;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;
using Vector2 = AmongUsClone.Shared.DataStructures.Vector2;

namespace AmongUsClone.Client.Networking
{
    public class PacketsReceiver : MonoBehaviour
    {
        private delegate void OnPacketReceivedCallback(Packet packet);

        private static readonly Dictionary<int, OnPacketReceivedCallback> PacketHandlers = new Dictionary<int, OnPacketReceivedCallback>
        {
            {(int) ServerPacketType.Welcome, ProcessWelcomePacket},
            {(int) ServerPacketType.PlayerConnected, ProcessPlayerConnectedPacket},
            {(int) ServerPacketType.PlayerDisconnected, ProcessPlayerDisconnectedPacket},
            {(int) ServerPacketType.PlayerPosition, ProcessPlayerPositionPacket},
        };

        public static void ProcessPacket(int packetTypeId, Packet packet)
        {
            PacketHandlers[packetTypeId](packet);
        }

        private static void ProcessWelcomePacket(Packet packet)
        {
            string message = packet.ReadString();
            int clientId = packet.ReadInt();

            // Todo: remove useless message?
            Debug.Log($"Message from server: {message}");
            Client.instance.id = clientId;
            PacketsSender.SendWelcomeReceivedPacket();

            Client.instance.udpConnectionToServer.Connect(((IPEndPoint) Client.instance.tcpConnectionToServer.tcpClient.Client.LocalEndPoint).Port);
        }

        private static void ProcessPlayerConnectedPacket(Packet packet)
        {
            int playerId = packet.ReadInt();
            string playerName = packet.ReadString();
            Vector2 playerPosition = packet.ReadVector2();
            
            GameManager.instance.AddPlayerToLobby(playerId, playerName, playerPosition);
        }
        
        private static void ProcessPlayerDisconnectedPacket(Packet packet)
        {
            int playerId = packet.ReadInt();
            
            GameManager.instance.RemovePlayerFromLobby(playerId);
        }

        private static void ProcessPlayerPositionPacket(Packet packet)
        {
            int playerId = packet.ReadInt();
            Vector2 playerPosition = packet.ReadVector2();

            GameManager.instance.UpdatePlayerPosition(playerId, playerPosition);
        }
    }
}
