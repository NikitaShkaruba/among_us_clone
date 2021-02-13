using System;
using System.Collections.Generic;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Snapshots;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using AmongUsClone.Shared.Scenes;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using Helpers = AmongUsClone.Shared.Helpers;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking.PacketManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PacketsReceiver", menuName = "PacketsReceiver")]
    public class PacketsReceiver : ScriptableObject
    {
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private NetworkSimulation networkSimulation;
        [SerializeField] private GameSnapshots gameSnapshots;
        [SerializeField] private ConnectionToServer connectionToServer;
        [SerializeField] private PlayersManager playersManager;

        private delegate void OnPacketReceivedCallback(Packet packet);

        private Dictionary<int, OnPacketReceivedCallback> packetHandlers;

        public void OnEnable()
        {
            packetHandlers = new Dictionary<int, OnPacketReceivedCallback>
            {
                {(int) ServerPacketType.Welcome, ProcessWelcomePacket},
                {(int) ServerPacketType.Kicked, ProcessKickedPacket},
                {(int) ServerPacketType.PlayerConnected, ProcessPlayerConnectedPacket},
                {(int) ServerPacketType.PlayerDisconnected, ProcessPlayerDisconnectedPacket},
                {(int) ServerPacketType.GameSnapshot, ProcessGameSnapshotPacket},
                {(int) ServerPacketType.ColorChanged, ProcessPlayerColorChangedPacket},
                {(int) ServerPacketType.GameStarts, ProcessGameStartsPacket},
                {(int) ServerPacketType.GameStarted, ProcessGameStartedPacket},
                {(int) ServerPacketType.SecurityCamerasEnabled, ProcessSecurityCamerasEnabledPacket},
                {(int) ServerPacketType.SecurityCamerasDisabled, ProcessSecurityCamerasDisabledPacket},
            };
        }

        public void ProcessPacket(int packetTypeId, Packet packet, bool isTcp)
        {
            string protocolName = isTcp ? "TCP" : "UDP";
            Logger.LogEvent(LoggerSection.Network, $"Received «{Helpers.GetEnumName((ServerPacketType) packetTypeId)}» {protocolName} packet from server");

            packetHandlers[packetTypeId](packet);
        }

        private void ProcessWelcomePacket(Packet packet)
        {
            Action action = () =>
            {
                int myPlayerId = packet.ReadInt();
                connectionToServer.FinishConnection(myPlayerId);

                Logger.LogEvent(LoggerSection.Connection, $"Connected successfully to server. My player id is {myPlayerId}");
            };

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessKickedPacket(Packet packet)
        {
            Action action = () =>
            {
                connectionToServer.Disconnect();
                Logger.LogEvent(LoggerSection.Connection, "Received a kick from server");
            };

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessPlayerConnectedPacket(Packet packet)
        {
            Action action = () =>
            {
                int playerId = packet.ReadInt();
                string playerName = packet.ReadString();
                bool isPlayerLobbyHost = packet.ReadBool();
                PlayerColor playerColor = (PlayerColor) packet.ReadInt();
                Vector2 playerPosition = packet.ReadVector2();
                bool playerLookingRight = packet.ReadBool();

                if (scenesManager.GetActiveScene() == Scene.MainMenu)
                {
                    mainMenuGamePhase.InitializeLobby(playerId, playerName, playerColor, playerPosition, playerLookingRight, isPlayerLobbyHost);
                }
                else
                {
                    lobbyGamePhase.AddPlayerToLobby(playerId, playerName, playerColor, playerPosition, playerLookingRight, isPlayerLobbyHost);
                }

                Logger.LogEvent(LoggerSection.Connection, $"Added player {playerId} to lobby");
            };

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessPlayerDisconnectedPacket(Packet packet)
        {
            Action action = () =>
            {
                int playerId = packet.ReadInt();
                playersManager.RemovePlayer(playerId);

                Logger.LogEvent(LoggerSection.Connection, $"Player {playerId} has disconnected");
            };

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessGameSnapshotPacket(Packet packet)
        {
            Action action = () =>
            {
                ClientGameSnapshot gameSnapshot = packet.ReadClientGameSnapshot();
                gameSnapshots.ProcessSnapshot(gameSnapshot);
            };

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessPlayerColorChangedPacket(Packet packet)
        {
            Action action = () =>
            {
                int playerId = packet.ReadInt();
                PlayerColor playerColor = (PlayerColor) packet.ReadInt();

                lobbyGamePhase.ChangePlayerColor(playerId, playerColor);
            };

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessGameStartsPacket(Packet packet)
        {
            Action action = () => lobbyGamePhase.InitiateGameStart();

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessGameStartedPacket(Packet packet)
        {
            Action action = () =>
            {
                List<int> impostorPlayerIds = new List<int>();

                bool isPlayingAsImpostor = packet.ReadBool();
                int impostorsAmount = packet.ReadInt();

                if (isPlayingAsImpostor)
                {
                    for (int impostorIndex = 0; impostorIndex < impostorsAmount; impostorIndex++)
                    {
                        int impostorPlayerId = packet.ReadInt();
                        impostorPlayerIds.Add(impostorPlayerId);
                    }
                }

                lobbyGamePhase.StartGame(isPlayingAsImpostor, impostorsAmount, impostorPlayerIds.ToArray());
            };

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessSecurityCamerasEnabledPacket(Packet packet)
        {
            Action action = () => playGamePhase.EnableSecurityCameras();

            networkSimulation.ReceiveThroughNetwork(action);
        }

        private void ProcessSecurityCamerasDisabledPacket(Packet packet)
        {
            Action action = () => playGamePhase.DisableSecurityCameras();

            networkSimulation.ReceiveThroughNetwork(action);
        }
    }
}
