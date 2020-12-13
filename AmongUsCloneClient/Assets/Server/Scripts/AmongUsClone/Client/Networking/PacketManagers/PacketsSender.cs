using AmongUsClone.Shared;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;

namespace AmongUsClone.Client.Networking.PacketManagers
{
    public class PacketsSender : MonoBehaviour
    {
        public static void SendWelcomeReceivedPacket()
        {
            const ClientPacketType clientPacketType = ClientPacketType.WelcomeReceived;

            using (Packet packet = new Packet((int) clientPacketType))
            {
                packet.Write(Game.instance.connectionToServer.myPlayerId);
                packet.Write(Game.instance.mainMenu.userNameField.text);

                Game.instance.connectionToServer.SendTcpPacket(clientPacketType, packet);
            }
        }

        public static void SendPlayerControlsPacket(PlayerControls playerControls)
        {
            const ClientPacketType clientPacketType = ClientPacketType.PlayerControls;

            using (Packet packet = new Packet((int) clientPacketType))
            {
                bool[] serializedPlayerControls = playerControls.Serialize();

                packet.Write(serializedPlayerControls.Length);

                foreach (bool control in serializedPlayerControls)
                {
                    packet.Write(control);
                }

                Game.instance.connectionToServer.SendUdpPacket(clientPacketType, packet);
            }
        }
    }
}