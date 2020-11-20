using UnityEngine;

namespace AmongUsClone.Client.Networking
{
    public class Client : MonoBehaviour
    {
        public static Client instance;

        [HideInInspector] public int id;
        public string ip = "127.0.0.1";
        public int port = 26950;
        public TcpConnectionToServer tcpConnectionToServer;
        public UdpConnectionToServer udpConnectionToServer;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying the object!");
                Destroy(this);
            }
        }

        private void Start()
        {
            tcpConnectionToServer = new TcpConnectionToServer();
            udpConnectionToServer = new UdpConnectionToServer();
        }

        public void ConnectToServer()
        {
            tcpConnectionToServer.Connect();
        }
    }
}
