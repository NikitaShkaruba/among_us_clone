using System.Net;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking
{
    public class ConnectionToServer
    {
        public const string ServerIP = "127.0.0.1";
        public const int ServerPort = 26950;

        public int myPlayerId;

        private TcpConnection tcpConnection;
        private UdpConnection udpConnection;
        private bool isConnected;

        public void Connect()
        {
            tcpConnection = new TcpConnection();
            udpConnection = null;

            isConnected = true;
        }

        public void FinishConnection(int myPlayerId)
        {
            this.myPlayerId = myPlayerId;
            PacketsSender.SendWelcomeReceivedPacket();

            udpConnection = new UdpConnection(GetTcpLocalEndpoint().Port);
        }

        public void Disconnect()
        {
            // This check and this variable is needed, because unity is not Closing instantly on Application.Quit();
            if (!isConnected)
            {
                return;
            }

            isConnected = false;

            tcpConnection.tcpClient.Close();
            tcpConnection = null;
            udpConnection.CloseConnection();
            udpConnection = null;

            Logger.LogEvent(LoggerSection.Connection, "Disconnected from the server");
        }

        public void SendTcpPacket(ClientPacketType clientPacketType, Packet packet)
        {
            packet.WriteLength();
            tcpConnection.SendPacket(packet);

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(clientPacketType)}» TCP packet to the server");
        }

        public void SendUdpPacket(ClientPacketType clientPacketType, Packet packet)
        {
            packet.WriteLength();
            udpConnection.SendPacket(packet);

            Logger.LogEvent(LoggerSection.Network, $"Sent «{Helpers.GetEnumName(clientPacketType)}» UDP packet to the server");
        }

        private IPEndPoint GetTcpLocalEndpoint()
        {
            return (IPEndPoint) tcpConnection.tcpClient.Client.LocalEndPoint;
        }
    }
}
