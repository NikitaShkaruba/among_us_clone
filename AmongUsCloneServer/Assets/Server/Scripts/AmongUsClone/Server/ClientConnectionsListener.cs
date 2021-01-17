using System;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking.Tcp;
using AmongUsClone.Server.Networking.Udp;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "ClientConnectionsListener", menuName = "ScriptableObjects/ClientConnectionsListener")]
    public class ClientConnectionsListener : ScriptableObject
    {
        [SerializeField] private UdpClient udpClient;
        [SerializeField] private TcpConnectionsListener tcpConnectionsListener;

        private const int Port = 26950;

        private void OnEnable()
        {
            tcpConnectionsListener.Initialize(Port);
            Logger.LogEvent(LoggerSection.Initialization, $"TCP connections listener started. Listening at port {Port}.");

            udpClient.Initialize(Port);
            Logger.LogEvent(LoggerSection.Initialization, $"UDP client started. Listening at port {Port}.");
        }

        public void StopListening()
        {
            tcpConnectionsListener.Stop();
            udpClient.StopListening();
        }
    }
}
