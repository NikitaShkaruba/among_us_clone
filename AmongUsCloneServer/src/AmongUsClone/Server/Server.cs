using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Server.Networking;

namespace AmongUsClone.Server
{
    public static class Server
    {
        private static bool isRunning = true;
        private const int TicksPerSecond = 30;
        private const int MillisecondsPerTick = 1000 / TicksPerSecond;

        public const int MinPlayerId = 1;
        public static int MaxPlayerId { get; private set; }

        private static TcpListener tcpListener;
        public static readonly Dictionary<int, Client> Clients = new Dictionary<int, Client>();
        private static int port;

        public static void Start(int maxPlayers, int port)
        {
            Logger.LogEvent("Starting server...");

            InitializeBase(maxPlayers, port);
            InitializeMainThread();
            InitializeTcpListener();
            InitializeUdpListener();

            Logger.LogEvent($"Server started. Listening on port {port}");
        }

        private static void InitializeBase(int maxPlayers, int port)
        {
            Console.Title = "Among Us Server";
            MaxPlayerId = maxPlayers;
            Server.port = port;
        }

        private static void InitializeMainThread()
        {
            Thread mainThread = new Thread(ExecuteMainThread);
            mainThread.Start();
        }

        private static void InitializeTcpListener()
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(OnTcpConnection, null);

            Logger.LogEvent($"TCP listener started");
        }

        private static void InitializeUdpListener()
        {
            UdpConnectionToClient.InitializeListener(port);
            Logger.LogEvent($"UDP listener started");
        }

        private static void ExecuteMainThread()
        {
            Logger.LogEvent($"Main thread started. Running at {TicksPerSecond} ticks per second");
            DateTime nextLoopDateTime = DateTime.Now;

            while (isRunning)
            {
                while (nextLoopDateTime < DateTime.Now)
                {
                    GameLogic.Update();

                    nextLoopDateTime = nextLoopDateTime.AddMilliseconds(MillisecondsPerTick);
                    if (nextLoopDateTime > DateTime.Now)
                    {
                        Thread.Sleep(nextLoopDateTime - DateTime.Now);
                    }
                }
            }
        }

        private static void OnTcpConnection(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            Logger.LogEvent($"Incoming tcp connection from {tcpClient.Client.RemoteEndPoint}...");

            // Start listening for the next client connection
            tcpListener.BeginAcceptTcpClient(OnTcpConnection, null);

            for (int clientId = MinPlayerId; clientId <= MaxPlayerId; clientId++)
            {
                // If this client exists already - skip this clientId in order to find a free one
                if (Clients.ContainsKey(clientId))
                {
                    continue;
                }

                Clients[clientId] = new Client(clientId);
                Clients[clientId].TcpConnectionToClient.Connect(tcpClient);
                return;
            }

            Logger.LogError($"{tcpClient.Client.RemoteEndPoint} failed to connect a client: Server is full");
        }
    }
}
