using System;
using System.Net;
using System.Net.Sockets;
using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Meta;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;
using UdpClient = AmongUsClone.Server.Networking.Udp.UdpClient;

namespace AmongUsClone.Server.Networking.Tcp
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "TcpConnectionsListener", menuName = "ScriptableObjects/TcpConnectionsListener")]
    public class TcpConnectionsListener : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private Game.PlayersManager playersManager;
        [SerializeField] private UdpClient udpClient;
        [SerializeField] private PacketsReceiver packetsReceiver;
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;

        private TcpListener tcpListener;

        public void Initialize(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(OnConnection, null);
        }

        private void OnConnection(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            Logger.LogEvent(LoggerSection.Network, $"Incoming tcp connection from {tcpClient.Client.RemoteEndPoint}...");

            // Start listening for the next client connection
            tcpListener.BeginAcceptTcpClient(OnConnection, null);

            for (int playerId = Game.PlayersManager.MinPlayerId; playerId <= Game.PlayersManager.MaxPlayerId; playerId++)
            {
                // If this client exists already - skip this playerId in order to find a free one
                if (playersManager.clients.ContainsKey(playerId))
                {
                    continue;
                }

                playersManager.clients[playerId] = new Client(playerId, udpClient, packetsReceiver, packetsSender, lobbyGamePhase, metaMonoBehaviours);
                playersManager.clients[playerId].ConnectTcp(tcpClient);
                return;
            }

            Logger.LogError(LoggerSection.Connection, $"{tcpClient.Client.RemoteEndPoint} failed to connect a client: Server is full");
        }

        public void Stop()
        {
            tcpListener.Stop();
        }
    }
}
