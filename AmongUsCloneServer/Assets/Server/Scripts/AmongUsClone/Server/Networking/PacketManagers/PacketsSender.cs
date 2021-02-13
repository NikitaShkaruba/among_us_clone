using System;
using System.Linq;
using System.Net.Http.Headers;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Snapshots;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Networking.PacketManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PacketsSender", menuName = "PacketsSender")]
    public class PacketsSender : ScriptableObject
    {
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private ClientGameSnapshotsManager clientGameSnapshotsManager;

        public void SendWelcomePacket(int playerId)
        {
            const ServerPacketType packetTypeId = ServerPacketType.Welcome;

            Packet packet = new Packet((int) packetTypeId);
            packet.Write(playerId);
            packet.Write(PlayersManager.MaxPlayerId + 1);

            SendTcpPacket(playerId, packetTypeId, packet);
        }

        public void SendKickedPacket(int playerId)
        {
            const ServerPacketType packetTypeId = ServerPacketType.Kicked;
            Packet packet = new Packet((int) packetTypeId);

            SendTcpPacketToAllExceptOne(playerId, packetTypeId, packet);
        }

        public void SendPlayerConnectedPacket(int playerId, ServerPlayer serverPlayer)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerConnected;

            Packet packet = new Packet((int) packetType);
            packet.Write(serverPlayer.basePlayer.information.id);
            packet.Write(serverPlayer.basePlayer.information.name);
            packet.Write(serverPlayer.remoteControllable.isLobbyHost);
            packet.Write((int) serverPlayer.basePlayer.colorable.color);
            packet.Write(serverPlayer.transform.position);
            packet.Write(!serverPlayer.spriteRenderer.flipX);

            SendTcpPacket(playerId, packetType, packet);
        }

        public void SendPlayerDisconnectedPacket(int playerId)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerDisconnected;

            Packet packet = new Packet((int) packetType);
            packet.Write(playerId);

            SendTcpPacketToAll(packetType, packet);
        }

        public void SendGameSnapshotPackets(GameSnapshot gameSnapshot)
        {
            const ServerPacketType packetType = ServerPacketType.GameSnapshot;

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                ClientGameSnapshot clientGameSnapshot = clientGameSnapshotsManager.CreateClientGameSnapshot(client, gameSnapshot);

                Packet packet = new Packet((int) packetType);
                packet.Write(clientGameSnapshot);

                SendUdpPacket(client.playerId, packetType, packet);
            }
        }

        public void SendColorChanged(int playerId, PlayerColor newPlayerColor)
        {
            const ServerPacketType packetType = ServerPacketType.ColorChanged;

            Packet packet = new Packet((int) packetType);
            packet.Write(playerId);
            packet.Write((int) newPlayerColor);

            SendTcpPacketToAll(packetType, packet);
        }

        public void SendGameStartsPacket()
        {
            const ServerPacketType packetType = ServerPacketType.GameStarts;

            Packet packet = new Packet((int) packetType);
            SendTcpPacketToAll(packetType, packet);
        }

        public void SendGameStartedPacket(int[] impostorPlayerIds)
        {
            const ServerPacketType packetType = ServerPacketType.GameStarted;

            foreach (int playerId in playersManager.clients.Keys)
            {
                bool isPlayerImposter = Array.Exists(impostorPlayerIds, imposterPlayerId => imposterPlayerId == playerId);

                Packet packet = new Packet((int) packetType);

                packet.Write(isPlayerImposter);
                packet.Write(impostorPlayerIds.Length);

                if (isPlayerImposter)
                {
                    foreach (int impostorPlayerId in impostorPlayerIds)
                    {
                        packet.Write(impostorPlayerId);
                    }
                }

                SendTcpPacket(playerId, packetType, packet);
            }
        }

        public void SendSecurityCamerasEnabledPacket()
        {
            const ServerPacketType packetType = ServerPacketType.SecurityCamerasEnabled;

            Packet packet = new Packet((int) packetType);
            SendTcpPacketToAll(packetType, packet);
        }

        public void SendSecurityCamerasDisabledPacket()
        {
            const ServerPacketType packetType = ServerPacketType.SecurityCamerasDisabled;

            Packet packet = new Packet((int) packetType);
            SendTcpPacketToAll(packetType, packet);
        }

        private void SendTcpPacket(int playerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            playersManager.clients[playerId].SendTcpPacket(packet);

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» TCP packet to the client {playerId}");
        }

        // ReSharper disable once UnusedMember.Local
        private void SendTcpPacketToAll(ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                client.SendTcpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» TCP packet to all clients");
        }

        // ReSharper disable once UnusedMember.Local
        private void SendTcpPacketToAllExceptOne(int ignoredPlayerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                if (client.playerId == ignoredPlayerId)
                {
                    continue;
                }

                client.SendTcpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» TCP packet to all clients except ${ignoredPlayerId}");
        }

        // ReSharper disable once UnusedMember.Local
        private void SendUdpPacket(int playerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();
            playersManager.clients[playerId].SendUdpPacket(packet);

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» UDP packet to the client {playerId}");
        }

        private void SendUdpPacketToAll(ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                client.SendUdpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» UDP packet to all clients");
        }

        // ReSharper disable once UnusedMember.Local
        private void SendUdpPacketToAllExceptOne(int ignoredPlayerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                if (client.playerId == ignoredPlayerId)
                {
                    continue;
                }

                client.SendUdpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» UDP packet to all clients except ${ignoredPlayerId}");
        }
    }
}
