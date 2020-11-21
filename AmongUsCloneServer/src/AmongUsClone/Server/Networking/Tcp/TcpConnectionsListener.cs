using System;
using System.Net;
using AmongUsClone.Server.Infrastructure;

namespace AmongUsClone.Server.Networking.Tcp
{
    public static class TcpConnectionsListener
    {
        private static System.Net.Sockets.TcpListener tcpListener;

        public static void Initialize(int port)
        {
            tcpListener = new System.Net.Sockets.TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(OnConnection, null);
        }

        private static void OnConnection(IAsyncResult result)
        {
            System.Net.Sockets.TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            Logger.LogEvent(LoggerSection.Network, $"Incoming tcp connection from {tcpClient.Client.RemoteEndPoint}...");

            // Start listening for the next client connection
            tcpListener.BeginAcceptTcpClient(OnConnection, null);

            for (int playerId = Server.MinPlayerId; playerId <= Server.MaxPlayerId; playerId++)
            {
                // If this client exists already - skip this playerId in order to find a free one
                if (Server.clients.ContainsKey(playerId))
                {
                    continue;
                }

                Server.clients[playerId] = new Client(playerId);
                Server.clients[playerId].ConnectTcp(tcpClient);
                return;
            }

            Logger.LogError(LoggerSection.ClientConnection, $"{tcpClient.Client.RemoteEndPoint} failed to connect a client: Server is full");
        }
    }
}
