using System.Net;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Networking.PacketTypes;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Helpers = AmongUsClone.Shared.Helpers;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    [CreateAssetMenu(fileName = "ConnectionToServer", menuName = "ConnectionToServer")]
    public class ConnectionToServer : ScriptableObject
    {
        public const string ServerIP = "127.0.0.1";
        public const int ServerPort = 26950;

        public int myPlayerId;

        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private PacketsReceiver packetsReceiver;
        [SerializeField] private PlayersManager playersManager;

        private TcpConnection tcpConnection;
        private UdpConnection udpConnection;
        private bool isConnected;

        public void Connect()
        {
            tcpConnection = new TcpConnection(this, packetsReceiver, metaMonoBehaviours);
            udpConnection = null;

            isConnected = true;
        }

        public void FinishConnection(int myPlayerId)
        {
            this.myPlayerId = myPlayerId;
            packetsSender.SendWelcomeReceivedPacket();

            udpConnection = new UdpConnection(this, metaMonoBehaviours, packetsReceiver, GetTcpLocalEndpoint().Port);
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

            ScenesManager.SwitchScene(Scene.MainMenu);
            playersManager.ClearPlayers();

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
            if (udpConnection == null)
            {
                Logger.LogNotice(LoggerSection.Network, "Unable to send udp packet, because udp connection is down");
                return;
            }

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
