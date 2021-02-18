using System;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;

namespace AmongUsClone.Client.Networking.PacketManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PacketsSender", menuName = "PacketsSender")]
    public class PacketsSender : ScriptableObject
    {
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;
        [SerializeField] private NetworkSimulation networkSimulation;
        [SerializeField] private ConnectionToServer connectionToServer;

        public void SendWelcomeReceivedPacket()
        {
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.WelcomeReceived;

                Packet packet = new Packet((int) clientPacketType);
                packet.Write(connectionToServer.myPlayerId);
                packet.Write(mainMenuGamePhase.mainMenu.userNameField.text);
                packet.Write(GameConfiguration.ApiVersion);

                connectionToServer.SendTcpPacket(clientPacketType, packet);
            };

            networkSimulation.SendThroughNetwork(action);
        }

        public void SendPlayerInputPacket(PlayerInput playerInput)
        {
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.PlayerInput;

                Packet packet = new Packet((int) clientPacketType);
                packet.Write(playerInput);

                connectionToServer.SendUdpPacket(clientPacketType, packet);
            };

            networkSimulation.SendThroughNetwork(action);
        }
    }
}
