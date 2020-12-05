using System;
using System.Collections.Generic;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.Tcp;
using AmongUsClone.Server.Networking.Udp;
using AmongUsClone.Shared;
using UnityEngine;
using Logger = AmongUsClone.Server.Infrastructure.Logger;

namespace AmongUsClone.Server
{
    public class Server : MonoBehaviour
    {
        public const int MinPlayerId = 1;
        public static int MaxPlayerId { get; private set; }

        public static readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public void Start()
        {
            Logger.LogEvent(LoggerSection.Initialization, "Starting the server...");

            Console.Title = "Among Us Server";
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
            const int port = 26950;
            MaxPlayerId = 10;
            Logger.LogEvent(LoggerSection.Initialization, $"Global variables initialized. Running at {Application.targetFrameRate} ticks per second.");

            TcpConnectionsListener.Initialize(port);
            Logger.LogEvent(LoggerSection.Initialization, $"TCP connections listener started. Listening at port {port}.");

            UdpClient.Initialize(port);
            Logger.LogEvent(LoggerSection.Initialization, $"UDP client started. Listening at port {port}.");

            Logger.LogEvent(LoggerSection.Initialization, "Server started.");
        }

        private void OnApplicationQuit()
        {
            TcpConnectionsListener.Stop();
            UdpClient.StopListening();
        }

        private void FixedUpdate()
        {
            ThreadManager.ExecuteMainThreadActions();
        }
    }
}
