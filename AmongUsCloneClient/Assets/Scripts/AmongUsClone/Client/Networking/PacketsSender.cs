using AmongUsClone.Client.UI;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;

namespace AmongUsClone.Client.Networking
{
    public class PacketsSender : MonoBehaviour
    {
        public static void SendWelcomeReceivedPacket()
        {
            using (Packet packet = new Packet((int) ClientPacketType.WelcomeReceived))
            {
                packet.Write(Client.Instance.id);
                packet.Write(UiManager.Instance.userNameField.text);

                SendTcpPacket(packet);
            }
        }

        public static void SendUdpTestReceived()
        {
            using (Packet packet = new Packet((int) ClientPacketType.UdpTestReceived))
            {
                packet.Write("Received a test UDP packet");
                SendUdpPacket(packet);
            }
        }

        private static void SendTcpPacket(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.TcpConnectionToServer.SendPacket(packet);
        }

        private static void SendUdpPacket(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.UdpConnectionToServer.SendPacket(packet);
        }
    }
}