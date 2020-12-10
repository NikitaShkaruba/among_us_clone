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

        public static void SendPlayerInputPacket(PlayerInput playerInput)
        {
            const ClientPacketType clientPacketType = ClientPacketType.PlayerInput;

            using (Packet packet = new Packet((int) clientPacketType))
            {
                bool[] serializedPlayerInput = playerInput.Serialize();

                packet.Write(serializedPlayerInput.Length);

                foreach (bool input in serializedPlayerInput)
                {
                    packet.Write(input);
                }

                Game.instance.connectionToServer.SendUdpPacket(clientPacketType, packet);
            }
        }
    }
}