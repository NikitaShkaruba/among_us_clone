using AmongUsClone.Client.UI;
using AmongUsClone.Shared;
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
                packet.Write(Client.instance.id);
                packet.Write(MainMenuManager.instance.userNameField.text);

                SendTcpPacket(packet);
            }
        }

        public static void SendPlayerInputPacket(PlayerInput playerInput)
        {
            bool[] serializedPlayerInput = playerInput.Serialize();
                
            using (Packet packet = new Packet((int) ClientPacketType.PlayerInput))
            {
                packet.Write(serializedPlayerInput.Length);
                foreach (bool input in serializedPlayerInput)
                {
                    packet.Write(input);
                }
                
                SendUdpPacket(packet);
            }
        }

        private static void SendTcpPacket(Packet packet)
        {
            packet.WriteLength();
            Client.instance.tcpConnectionToServer.SendPacket(packet);
        }

        private static void SendUdpPacket(Packet packet)
        {
            packet.WriteLength();
            Client.instance.udpConnectionToServer.SendPacket(packet);
        }
    }
}