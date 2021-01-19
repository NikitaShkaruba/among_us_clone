using System;
using System.Net;
using System.Net.Sockets;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Networking;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking
{
    public class UdpConnection
    {
        private readonly UdpClient udpClient;
        private IPEndPoint ipEndPoint;

        private ConnectionToServer connectionToServer;
        private MetaMonoBehaviours metaMonoBehaviours;
        private PacketsReceiver packetsReceiver;

        public UdpConnection(ConnectionToServer connectionToServer, MetaMonoBehaviours metaMonoBehaviours, PacketsReceiver packetsReceiver, int localPort)
        {
            this.connectionToServer = connectionToServer;
            this.metaMonoBehaviours = metaMonoBehaviours;
            this.packetsReceiver = packetsReceiver;

            ipEndPoint = new IPEndPoint(IPAddress.Parse(ConnectionToServer.ServerIP), ConnectionToServer.ServerPort);

            udpClient = new UdpClient(localPort);
            udpClient.Connect(ipEndPoint);
            udpClient.BeginReceive(OnConnection, null);
            Logger.LogEvent(LoggerSection.Connection, "Started listening for udp connections");

            Packet packet = new Packet();
            SendPacket(packet);
            Logger.LogEvent(LoggerSection.Connection, "Sent first empty udp packet to connect with server's udp");
        }

        public void SendPacket(Packet packet)
        {
            if (udpClient == null)
            {
                throw new Exception("Uninitialized UdpClient");
            }

            try
            {
                packet.InsertInt(connectionToServer.myPlayerId);
                udpClient.BeginSend(packet.ToArray(), packet.GetLength(), null, null);
            }
            catch (Exception exception)
            {
                Logger.LogError(LoggerSection.Network, $"Error sending data through udp: {exception}");
            }
        }

        public void CloseConnection()
        {
            udpClient.Close();
        }

        private void OnConnection(IAsyncResult result)
        {
            try
            {
                byte[] data = udpClient.EndReceive(result, ref ipEndPoint);

                // Start listening for next connection again
                udpClient.BeginReceive(OnConnection, null);

                if (data.Length < sizeof(int))
                {
                    connectionToServer.Disconnect();
                    return;
                }

                HandlePacketData(data);
            }
            catch
            {
                connectionToServer.Disconnect();
            }
        }

        private void HandlePacketData(byte[] data)
        {
            Packet packet = new Packet(data);
            packet.ReadInt(); // Read not needed 'packet length' in order to update read position

            metaMonoBehaviours.applicationCallbacks.ScheduleFixedUpdateAction(() =>
            {
                int packetTypeId = packet.ReadInt();
                packetsReceiver.ProcessPacket(packetTypeId, packet, false);
            });
        }
    }
}
