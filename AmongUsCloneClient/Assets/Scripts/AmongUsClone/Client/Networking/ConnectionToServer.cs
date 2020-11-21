using System.Net;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Networking;
using UnityEngine;

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

            Debug.Log("Disconnected from the server");
        }

        public void SendTcpPacket(Packet packet)
        {
            packet.WriteLength();
            tcpConnection.SendPacket(packet);
        }

        public void SendUdpPacket(Packet packet)
        {
            packet.WriteLength();
            udpConnection.SendPacket(packet);
        }

        private IPEndPoint GetTcpLocalEndpoint()
        {
            return (IPEndPoint) tcpConnection.tcpClient.Client.LocalEndPoint;
        }
    }
}
