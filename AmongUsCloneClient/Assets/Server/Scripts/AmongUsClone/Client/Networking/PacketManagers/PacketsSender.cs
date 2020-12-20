using AmongUsClone.Client.Game;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game.PlayerLogic;
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
                packet.Write(GameManager.instance.connectionToServer.myPlayerId);
                packet.Write(GameManager.instance.mainMenu.userNameField.text);

                GameManager.instance.connectionToServer.SendTcpPacket(clientPacketType, packet);
            }
        }

        public static void SendPlayerControlsPacket(int controlsRequestId, PlayerControls playerControls)
        {
            const ClientPacketType clientPacketType = ClientPacketType.PlayerControls;

            using (Packet packet = new Packet((int) clientPacketType))
            {
                packet.Write(controlsRequestId);

                bool[] serializedPlayerControls = playerControls.Serialize();
                packet.Write(serializedPlayerControls.Length);

                foreach (bool control in serializedPlayerControls)
                {
                    packet.Write(control);
                }

                GameManager.instance.connectionToServer.SendUdpPacket(clientPacketType, packet);
            }
        }
    }
}