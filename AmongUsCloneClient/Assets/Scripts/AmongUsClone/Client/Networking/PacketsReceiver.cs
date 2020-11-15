using System.Collections.Generic;
using System.Net;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;

namespace AmongUsClone.Client.Networking
{
    public class PacketsReceiver : MonoBehaviour
    {
        private delegate void OnPacketReceivedCallback(Packet packet);

        private static readonly Dictionary<int, OnPacketReceivedCallback> PacketHandlers = new Dictionary<int, OnPacketReceivedCallback>
        {
            {(int) ServerPacketType.Welcome, ProcessWelcomePacket},
            {(int) ServerPacketType.UdpTest, ProcessUdpTestReceiver}
        };

        public static void ProcessPacket(int packetTypeId, Packet packet)
        {
            PacketHandlers[packetTypeId](packet);
        }

        private static void ProcessWelcomePacket(Packet packet)
        {
            string message = packet.ReadString();
            int id = packet.ReadInt();

            // Todo: remove useless message?
            Debug.Log($"Message from server: {message}");
            Client.Instance.id = id;
            PacketsSender.SendWelcomeReceivedPacket();

            // Connect with the server via udp
            Client.Instance.UdpConnectionToServer.Connect(((IPEndPoint) Client.Instance.TcpConnectionToServer.TcpClient.Client.LocalEndPoint).Port);
        }

        private static void ProcessUdpTestReceiver(Packet packet)
        {
            string message = packet.ReadString();

            Debug.Log($"Received a packet via UDP: Contains message: {message}");
            PacketsSender.SendUdpTestReceived();
        }
    }
}
