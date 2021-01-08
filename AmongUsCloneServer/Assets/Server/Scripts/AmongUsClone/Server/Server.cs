using System;
using System.Collections;
using System.Collections.Generic;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.Tcp;
using AmongUsClone.Server.Networking.Udp;
using AmongUsClone.Server.Snapshots;
using AmongUsClone.Shared;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server
{
    public class Server : MonoBehaviour
    {
        public const int MinPlayerId = 0;
        public static int MaxPlayerId { get; private set; }
        public const int MinRequiredPlayersAmountForGame = 4;

        public static readonly Dictionary<int, Client> clients = new Dictionary<int, Client>();

        public void Start()
        {
            Logger.Initialize(new[] {LoggerSection.Network, LoggerSection.GameSnapshots}, true);
            Logger.LogEvent(LoggerSection.Initialization, "Starting the server...");

            Console.Title = "Among Us Server";
            const int port = 26950;
            MaxPlayerId = 9;
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
            MainThread.Execute();
            StartCoroutine(PostFixedUpdate());
        }

        private static IEnumerator PostFixedUpdate()
        {
            yield return new WaitForFixedUpdate();

            GameSnapshots.ProcessSnapshot();
        }
    }
}
