using System;
using System.Collections.Generic;
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

        public static readonly Dictionary<int, Client> Clients = new Dictionary<int, Client>();
        private static int port;

        public static void Start(int maxPlayers, int port)
        {
            Logger.LogEvent(LoggerSection.Initialization, "Starting server...");

            InitializeStaticVariables(maxPlayers, port);
            InitializeMainThread();
            InitializeTcpListener();
            InitializeUdpListener();

            Logger.LogEvent(LoggerSection.Initialization, $"Server started. Listening on port {port}.");
        }

        private static void InitializeStaticVariables(int maxPlayers, int port)
        {
            Console.Title = "Among Us Server";
            MaxPlayerId = maxPlayers;
            Server.port = port;
            
            Logger.LogEvent(LoggerSection.Initialization, $"Static variables initialized.");
        }

        private static void InitializeMainThread()
        {
            Thread mainThread = new Thread(ExecuteMainThread);
            mainThread.Start();
        }

        private static void InitializeTcpListener()
        {
            TcpConnectionToClient.InitializeListener(port);
            Logger.LogEvent(LoggerSection.Initialization, $"TCP listener started.");
        }

        private static void InitializeUdpListener()
        {
            UdpConnectionToClient.InitializeListener(port);
            Logger.LogEvent(LoggerSection.Initialization, $"UDP listener started.");
        }

        private static void ExecuteMainThread()
        {
            Logger.LogEvent(LoggerSection.Initialization, $"Main thread started. Running at {TicksPerSecond} ticks per second.");
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
    }
}
