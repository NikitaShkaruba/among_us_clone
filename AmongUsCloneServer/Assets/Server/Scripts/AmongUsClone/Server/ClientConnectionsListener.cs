using System;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking.Tcp;
using AmongUsClone.Server.Networking.Udp;
using AmongUsClone.Shared.Meta;
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
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;

        private const int Port = 26950;

        public void StartListening()
        {
            tcpConnectionsListener.Initialize(Port);
            Logger.LogEvent(LoggerSection.Initialization, $"TCP connections listener started. Listening at port {Port}.");

            udpClient.Initialize(Port);
            Logger.LogEvent(LoggerSection.Initialization, $"UDP client started. Listening at port {Port}.");

            var test = metaMonoBehaviours.applicationCallbacks;
            test.ScheduleOnApplicationQuitActions(StopListening);
        }

        private void StopListening()
        {
            tcpConnectionsListener.Stop();
            udpClient.StopListening();
        }
    }
}
