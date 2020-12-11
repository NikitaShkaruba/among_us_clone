using System.Net;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Networking.Udp;
using AmongUsClone.Shared.Networking;
using TcpClient = System.Net.Sockets.TcpClient;

namespace AmongUsClone.Server.Networking
{
    /**
     * Client is a player + networking information about how to communicate with him
     */
    public class Client
    {
        public readonly int playerId;
        public Player player;

        private Tcp.TcpConnection tcpConnection;
        private IPEndPoint udpIpEndPoint;

        public Client(int playerId)
        {
            this.playerId = playerId;

            tcpConnection = null;
            udpIpEndPoint = null;
        }

        public void ConnectTcp(TcpClient tcpClient)
        {
            tcpConnection = new Tcp.TcpConnection(playerId, tcpClient);
            tcpConnection.InitializeCommunication();
        }

        public void ConnectUdp(IPEndPoint clientIpEndPoint)
        {
            udpIpEndPoint = clientIpEndPoint;
        }

        public void SendTcpPacket(Packet packet)
        {
            tcpConnection.SendPacket(packet);
        }

        public void SendUdpPacket(Packet packet)
        {
            UdpClient.SendPacket(packet, udpIpEndPoint);
        }

        public EndPoint GetTcpEndPoint()
        {
            return tcpConnection.tcpClient.Client.RemoteEndPoint;
        }

        public bool IsConnectedViaUdp()
        {
            return udpIpEndPoint != null;
        }

        public bool IsCorrectUdpIpEndPoint(IPEndPoint clientIpEndPoint)
        {
            return udpIpEndPoint.ToString() == clientIpEndPoint.ToString();
        }

        public bool IsPlayerInitialized()
        {
            return player != null;
        }
    }
}
