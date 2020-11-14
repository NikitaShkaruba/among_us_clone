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

                SendPacket(packet);
            }
        }

        private static void SendPacket(Packet packet)
        {
            packet.WriteLength();
            Client.Instance.TcpConnectionToServer.SendPacket(packet);
        }
    }
}