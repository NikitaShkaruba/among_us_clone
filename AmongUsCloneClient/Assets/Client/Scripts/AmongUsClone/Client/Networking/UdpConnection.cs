using System;
using System.Net;
using System.Net.Sockets;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Networking;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking
{
    public class UdpConnection
    {
        private readonly UdpClient udpClient;
        private IPEndPoint ipEndPoint;

        public UdpConnection(int localPort)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ConnectionToServer.ServerIP), ConnectionToServer.ServerPort);

            udpClient = new UdpClient(localPort);
            udpClient.Connect(ipEndPoint);
            udpClient.BeginReceive(OnConnection, null);

            Packet packet = new Packet();
            SendPacket(packet);
        }

        public void SendPacket(Packet packet)
        {
            if (udpClient == null)
            {
                throw new Exception("Uninitialized UdpClient");
            }

            try
            {
                packet.InsertInt(GameManager.instance.connectionToServer.myPlayerId);
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
                    GameManager.instance.DisconnectFromLobby();
                    return;
                }

                HandlePacketData(data);
            }
            catch
            {
                GameManager.instance.DisconnectFromLobby();
            }
        }

        private static void HandlePacketData(byte[] data)
        {
            Packet initialPacket = new Packet(data);

            int packetLength = initialPacket.ReadInt();
            data = initialPacket.ReadBytes(packetLength);

            MainThread.ScheduleExecution(() =>
            {
                Packet packet = new Packet(data);
                int packetTypeId = packet.ReadInt();
                PacketsReceiver.ProcessPacket(packetTypeId, packet, false);
            });
        }
    }
}
