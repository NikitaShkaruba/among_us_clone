using System;
using System.Linq;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using AmongUsClone.Shared.Snapshots;

namespace AmongUsClone.Server.Networking.PacketManagers
{
    public static class PacketsSender
    {
        public static void SendWelcomePacket(int playerId)
        {
            const ServerPacketType packetTypeId = ServerPacketType.Welcome;

            Packet packet = new Packet((int) packetTypeId);
            packet.Write(playerId);
            packet.Write(Server.MaxPlayerId + 1);
            packet.Write(GameManager.MinRequiredPlayersAmountForGame);
            packet.Write(GameManager.SecondsForGameLaunch);

            SendTcpPacket(playerId, packetTypeId, packet);
        }

        public static void SendKickedPacket(int playerId)
        {
            const ServerPacketType packetTypeId = ServerPacketType.Kicked;
            Packet packet = new Packet((int) packetTypeId);

            SendTcpPacketToAllExceptOne(playerId, packetTypeId, packet);
        }

        public static void SendPlayerConnectedPacket(int playerId, Player player)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerConnected;

            Packet packet = new Packet((int) packetType);
            packet.Write(player.information.id);
            packet.Write(player.information.name);
            packet.Write(player.information.isLobbyHost);
            packet.Write((int) player.colorable.color);
            packet.Write(player.information.transform.position);

            SendTcpPacket(playerId, packetType, packet);
        }

        public static void SendPlayerDisconnectedPacket(int playerId)
        {
            const ServerPacketType packetType = ServerPacketType.PlayerDisconnected;

            Packet packet = new Packet((int) packetType);
            packet.Write(playerId);

            SendTcpPacketToAll(packetType, packet);
        }

        public static void SendGameSnapshotPackets(GameSnapshot gameSnapshot)
        {
            const ServerPacketType packetType = ServerPacketType.GameSnapshot;

            foreach (Client client in Server.clients.Values)
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                ClientGameSnapshot clientGameSnapshot = new ClientGameSnapshot(gameSnapshot, client.player.remoteControllable.lastProcessedInputId);

                Packet packet = new Packet((int) packetType);
                packet.Write(clientGameSnapshot);

                SendUdpPacket(client.playerId, packetType, packet);
            }
        }

        public static void SendColorChanged(int playerId, PlayerColor newPlayerColor)
        {
            const ServerPacketType packetType = ServerPacketType.ColorChanged;

            Packet packet = new Packet((int) packetType);
            packet.Write(playerId);
            packet.Write((int) newPlayerColor);

            SendTcpPacketToAll(packetType, packet);
        }

        public static void SendGameStartsPacket()
        {
            const ServerPacketType packetType = ServerPacketType.GameStarts;

            Packet packet = new Packet((int) packetType);
            SendTcpPacketToAll(packetType, packet);
        }

        public static void SendGameStartedPacket(int[] impostorPlayerIds)
        {
            const ServerPacketType packetType = ServerPacketType.GameStarted;

            foreach (int playerId in Server.clients.Keys)
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

        private static void SendTcpPacket(int playerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            Server.clients[playerId].SendTcpPacket(packet);

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» TCP packet to the client {playerId}");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendTcpPacketToAll(ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.clients.Values)
            {
                client.SendTcpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» TCP packet to all clients");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendTcpPacketToAllExceptOne(int ignoredPlayerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.clients.Values)
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
        private static void SendUdpPacket(int playerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();
            Server.clients[playerId].SendUdpPacket(packet);

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» UDP packet to the client {playerId}");
        }

        private static void SendUdpPacketToAll(ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.clients.Values)
            {
                client.SendUdpPacket(packet);
            }

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(serverPacketType)}» UDP packet to all clients");
        }

        // ReSharper disable once UnusedMember.Local
        private static void SendUdpPacketToAllExceptOne(int ignoredPlayerId, ServerPacketType serverPacketType, Packet packet)
        {
            packet.WriteLength();

            foreach (Client client in Server.clients.Values)
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
