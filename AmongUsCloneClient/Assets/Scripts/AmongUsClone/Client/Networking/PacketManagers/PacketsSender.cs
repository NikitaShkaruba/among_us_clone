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
            using (Packet packet = new Packet((int) ClientPacketType.WelcomeReceived))
            {
                packet.Write(Game.instance.connectionToServer.myPlayerId);
                packet.Write(Game.instance.mainMenu.userNameField.text);

                Game.instance.connectionToServer.SendTcpPacket(packet);
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

                Game.instance.connectionToServer.SendUdpPacket(packet);
            }
        }
    }
}