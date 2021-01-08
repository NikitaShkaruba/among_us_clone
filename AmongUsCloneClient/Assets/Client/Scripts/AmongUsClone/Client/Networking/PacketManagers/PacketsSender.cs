using System;
using AmongUsClone.Client.Game;
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
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.WelcomeReceived;

                Packet packet = new Packet((int) clientPacketType);
                packet.Write(GameManager.instance.connectionToServer.myPlayerId);
                packet.Write(GameManager.instance.mainMenu.userNameField.text);

                GameManager.instance.connectionToServer.SendTcpPacket(clientPacketType, packet);
            };

            NetworkSimulation.instance.SendThroughNetwork(action);
        }

        public static void SendPlayerInputPacket(PlayerInput playerInput)
        {
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.PlayerInput;

                Packet packet = new Packet((int) clientPacketType);
                packet.Write(playerInput);

                GameManager.instance.connectionToServer.SendUdpPacket(clientPacketType, packet);
            };

            NetworkSimulation.instance.SendThroughNetwork(action);
        }

        public static void SendColorChangeRequestPacket()
        {
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.ColorChangeRequest;

                Packet packet = new Packet((int) clientPacketType);
                GameManager.instance.connectionToServer.SendTcpPacket(clientPacketType, packet);
            };

            NetworkSimulation.instance.SendThroughNetwork(action);
        }

        public static void SendStartGamePacket()
        {
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.StartGame;

                Packet packet = new Packet((int) clientPacketType);
                GameManager.instance.connectionToServer.SendTcpPacket(clientPacketType, packet);
            };

            NetworkSimulation.instance.SendThroughNetwork(action);
        }
    }
}
