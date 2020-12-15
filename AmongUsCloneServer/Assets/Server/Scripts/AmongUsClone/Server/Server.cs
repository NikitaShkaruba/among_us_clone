using System;
using System.Collections;
using System.Collections.Generic;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.Tcp;
using AmongUsClone.Server.Networking.Udp;
using AmongUsClone.Server.Snapshots;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

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
            MainThread.Execute();
            StartCoroutine(PostFixedUpdate());
        }

        private static IEnumerator PostFixedUpdate()
        {
            yield return new WaitForFixedUpdate();

            GameSnapshotsManager.ProcessCurrentGameSnapshot();
        }
    }
}
