using System;
using System.Collections.Generic;
using System.Threading;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.Tcp;
using AmongUsClone.Server.Networking.Udp;
using AmongUsClone.Shared;

namespace AmongUsClone.Server
{
    public static class Server
    {
        public const int TicksPerSecond = 30;
        private const int MillisecondsPerTick = 1000 / TicksPerSecond;
        public const int MinPlayerId = 1;
        public static int MaxPlayerId { get; private set; }

        public static readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        // ReSharper disable once ConvertToConstant.Local
        private static bool isRunning = true;

        public static void Start(int maxPlayers, int port)
        {
            Logger.LogEvent(LoggerSection.Initialization, "Starting the server...");

            Console.Title = "Among Us Server";
            MaxPlayerId = maxPlayers;

            Thread mainThread = new Thread(ExecuteMainThread);
            mainThread.Start();
            Logger.LogEvent(LoggerSection.Initialization, $"Main thread started. Running at {TicksPerSecond} ticks per second.");

            TcpConnectionsListener.Initialize(port);
            Logger.LogEvent(LoggerSection.Initialization, $"TCP connections listener started. Listening on port {port}.");

            UdpClient.Initialize(port);
            Logger.LogEvent(LoggerSection.Initialization, $"UDP client started. Listening on port {port}.");

            Logger.LogEvent(LoggerSection.Initialization, "Server started.");
        }

        private static void ExecuteMainThread()
        {
            DateTime nextLoopDateTime = DateTime.Now;

            while (isRunning)
            {
                while (nextLoopDateTime < DateTime.Now)
                {
                    Tick();

                    nextLoopDateTime = nextLoopDateTime.AddMilliseconds(MillisecondsPerTick);
                    if (nextLoopDateTime > DateTime.Now)
                    {
                        Thread.Sleep(nextLoopDateTime - DateTime.Now);
                    }
                }
            }
        }

        private static void Tick()
        {
            // We need to lock the collection, because in multi-thread environment it can be modified while being iterated
            // Todo: properly handle locks
            lock (clients)
            {
                foreach (Client client in clients.Values)
                {
                    client.player?.Update();
                }
            }

            ThreadManager.UpdateMain();
        }
    }
}
