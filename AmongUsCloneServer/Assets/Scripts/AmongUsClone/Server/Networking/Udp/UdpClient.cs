using System;
using System.Net;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Server.Networking.Udp
{
    public static class UdpClient
    {
        private static System.Net.Sockets.UdpClient udpClient;

        public static void Initialize(int port)
        {
            udpClient = new System.Net.Sockets.UdpClient(port);
            udpClient.BeginReceive(OnConnection, null);
        }

        public static void SendPacket(Packet packet, IPEndPoint ipEndPoint)
        {
            // Because of a multithreading we can have a client, but he might not have an ipEndPoint
            if (ipEndPoint == null)
            {
                return;
            }

            try
            {
                udpClient.BeginSend(packet.ToArray(), packet.GetLength(), ipEndPoint, null, null);
            }
            catch (Exception exception)
            {
                Logger.LogError(LoggerSection.Network, $"Error sending UDP data to {ipEndPoint}: {exception}");
            }
        }

        private static void OnConnection(IAsyncResult result)
        {
            try
            {
                IPEndPoint clientIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.EndReceive(result, ref clientIpEndPoint);

                // Start listening for the next client connection
                udpClient.BeginReceive(OnConnection, null);

                if (data.Length < sizeof(int))
                {
                    return;
                }

                using (Packet packet = new Packet(data))
                {
                    int playerId = packet.ReadInt();

                    if (playerId == 0)
                    {
                        Logger.LogError(LoggerSection.Network, "Undefined client id in UDP packet");
                        return;
                    }

                    if (!Server.clients[playerId].IsConnectedViaUdp())
                    {
                        Server.clients[playerId].ConnectUdp(clientIpEndPoint);
                        return;
                    }

                    if (!Server.clients[playerId].IsCorrectUdpIpEndPoint(clientIpEndPoint))
                    {
                        Logger.LogError(LoggerSection.Network, "Hacking attempt, client ids doesn't match");
                        return;
                    }

                    HandlePacketData(playerId, packet);
                }
            }
            catch (Exception exception)
            {
                // Todo: handle randomly occured exception on OnConnection
                Logger.LogError(LoggerSection.Network, $"Error receiving UDP data: {exception}");
            }
        }

        private static void HandlePacketData(int playerId, Packet packet)
        {
            int packetLength = packet.ReadInt();
            byte[] packetBytes = packet.ReadBytes(packetLength);

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet newPacket = new Packet(packetBytes))
                {
                    int packetTypeId = newPacket.ReadInt();
                    PacketsReceiver.ProcessPacket(playerId, packetTypeId, newPacket, false);
                }
            });
        }

        public static void StopListening()
        {
            udpClient.Close();
        }
    }
}
