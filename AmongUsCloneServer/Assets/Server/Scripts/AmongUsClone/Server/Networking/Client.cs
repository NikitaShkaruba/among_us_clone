using System.Net;
using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Server.Networking.Tcp;
using AmongUsClone.Server.Networking.Udp;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Meta;
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

        private readonly MetaMonoBehaviours metaMonoBehaviours;
        private readonly TcpConnectionsListener tcpConnectionsListener;
        private readonly UdpClient udpClient;
        private readonly PacketsReceiver packetsReceiver;
        private readonly PacketsSender packetsSender;
        private readonly LobbyGamePhase lobbyGamePhase;

        public Client(int playerId, UdpClient udpClient, PacketsReceiver packetsReceiver, PacketsSender packetsSender, LobbyGamePhase lobbyGamePhase, MetaMonoBehaviours metaMonoBehaviours)
        {
            this.playerId = playerId;

            tcpConnection = null;
            udpIpEndPoint = null;

            this.metaMonoBehaviours = metaMonoBehaviours;
            this.udpClient = udpClient;
            this.packetsReceiver = packetsReceiver;
            this.packetsSender = packetsSender;
            this.lobbyGamePhase = lobbyGamePhase;
        }

        /**
         * Client may be connected with tcp, udp, but he is still not spawned at the game
         */
        public bool IsFullyInitialized()
        {
            return player != null;
        }

        public void FinishInitialization(Player player)
        {
            this.player = player;
        }

        public void ConnectTcp(TcpClient tcpClient)
        {
            tcpConnection = new Tcp.TcpConnection(playerId, tcpClient, packetsReceiver, packetsSender, lobbyGamePhase, metaMonoBehaviours);
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
            if (!IsConnectedViaUdp())
            {
                Logger.LogEvent(LoggerSection.Network, $"Skipped sending udp packet to client {playerId}, because it is not connected via udp yet");
                return;
            }

            udpClient.SendPacket(packet, udpIpEndPoint);
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
    }
}
