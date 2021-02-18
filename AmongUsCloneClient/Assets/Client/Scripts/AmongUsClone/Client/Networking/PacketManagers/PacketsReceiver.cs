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
        [SerializeField] private NetworkSimulation networkSimulation;
        [SerializeField] private GameSnapshots gameSnapshots;
        [SerializeField] private ConnectionToServer connectionToServer;

        private delegate void OnPacketReceivedCallback(Packet packet);

        private Dictionary<int, OnPacketReceivedCallback> packetHandlers;

        public void OnEnable()
        {
            packetHandlers = new Dictionary<int, OnPacketReceivedCallback>
            {
                {(int) ServerPacketType.Welcome, ProcessWelcomePacket},
                {(int) ServerPacketType.Kicked, ProcessKickedPacket},
                {(int) ServerPacketType.GameSnapshot, ProcessGameSnapshotPacket},
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

        private void ProcessGameSnapshotPacket(Packet packet)
        {
            Action action = () =>
            {
                ClientGameSnapshot gameSnapshot = packet.ReadClientGameSnapshot();
                gameSnapshots.ProcessSnapshot(gameSnapshot);
            };

            networkSimulation.ReceiveThroughNetwork(action);
        }
    }
}
