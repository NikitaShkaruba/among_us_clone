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
            {(int) ServerPacketType.SpawnPlayer, ProcessSpawnPlayerPacket},
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
            Client.Instance.id = clientId;
            PacketsSender.SendWelcomeReceivedPacket();

            // Connect with the server via udp
            Client.Instance.UdpConnectionToServer.Connect(((IPEndPoint) Client.Instance.TcpConnectionToServer.TcpClient.Client.LocalEndPoint).Port);
        }

        private static void ProcessSpawnPlayerPacket(Packet packet)
        {
            int playerId = packet.ReadInt();
            string playerName = packet.ReadString();
            Vector2 playerPosition = packet.ReadVector2();
            
            GameManager.Instance.SpawnPlayer(playerId, playerName, playerPosition);
        }
        
        private static void ProcessPlayerPositionPacket(Packet packet)
        {
            int playerId = packet.ReadInt();
            Vector2 playerPosition = packet.ReadVector2();

            if (!GameManager.Players.ContainsKey(playerId))
            {
                return;
            }
            
            GameManager.Players[playerId].transform.position = new Vector3(playerPosition.x, playerPosition.y, 0);
        }
    }
}
