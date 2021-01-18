using System;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using UnityEngine;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking.PacketManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    [CreateAssetMenu(fileName = "PacketsSender", menuName = "PacketsSender")]
    public class PacketsSender : ScriptableObject
    {
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private NetworkSimulation networkSimulation;

        public void SendWelcomeReceivedPacket()
        {
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.WelcomeReceived;

                Packet packet = new Packet((int) clientPacketType);
                packet.Write(mainMenuGamePhase.connectionToServer.myPlayerId);
                MainMenu mainMenu = mainMenuGamePhase.mainMenu;
                Logger.LogDebug($"mainMenu is null: {mainMenuGamePhase == null}");
                InputField mainMenuUserNameField = mainMenu.userNameField;
                packet.Write(mainMenuUserNameField.text);

                mainMenuGamePhase.connectionToServer.SendTcpPacket(clientPacketType, packet);
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

                lobbyGamePhase.connectionToServer.SendUdpPacket(clientPacketType, packet);
            };

            networkSimulation.SendThroughNetwork(action);
        }

        public void SendColorChangeRequestPacket()
        {
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.ColorChangeRequest;

                Packet packet = new Packet((int) clientPacketType);
                lobbyGamePhase.connectionToServer.SendTcpPacket(clientPacketType, packet);
            };

            networkSimulation.SendThroughNetwork(action);
        }

        public void SendStartGamePacket()
        {
            Action action = () =>
            {
                const ClientPacketType clientPacketType = ClientPacketType.StartGame;

                Packet packet = new Packet((int) clientPacketType);
                lobbyGamePhase.connectionToServer.SendTcpPacket(clientPacketType, packet);
            };

            networkSimulation.SendThroughNetwork(action);
        }
    }
}
