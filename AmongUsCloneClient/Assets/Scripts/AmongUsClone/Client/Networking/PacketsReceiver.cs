using System.Collections.Generic;
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
            {(int)ServerPacketType.Welcome, ProcessWelcomePacket}
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
        }
    }
}
