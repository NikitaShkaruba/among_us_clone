using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Server.Networking;

namespace AmongUsClone.Server
{
    public static class Server
    {
        private static int MaxPlayers { get; set; }
        private static int Port { get; set; }

        private static TcpListener tcpListener;
        private static readonly Dictionary<int, Client> Clients = new Dictionary<int, Client>();
        
        public static void Start(int maxPlayers, int port)
        {
            MaxPlayers = maxPlayers;
            Port = port;

            Logger.LogEvent("Starting server...");

            tcpListener = new TcpListener(IPAddress.Any, Port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);

            Logger.LogEvent($"Server is started on {Port}");
        }

        private static void TcpConnectCallback(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            tcpListener.BeginAcceptTcpClient(TcpConnectCallback, null);
            Logger.LogEvent($"Incoming connection from {tcpClient.Client.RemoteEndPoint}...");

            for (int clientId = 1; clientId <= MaxPlayers; clientId++)
            {
                if (!Clients.ContainsKey(clientId))
                {
                    Clients[clientId] = new Client(clientId);
                    Clients[clientId].TcpConnection.Connect(tcpClient);
                    return;
                }
            }

            Logger.LogError($"{tcpClient.Client.RemoteEndPoint} failed to connect a client: Server is full");
        }
    }
}
