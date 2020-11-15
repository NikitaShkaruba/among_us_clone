using UnityEngine;

namespace AmongUsClone.Client.Networking
{
    public class Client : MonoBehaviour
    {
        public static Client Instance;

        [HideInInspector] public int id = 0;
        public string ip = "127.0.0.1";
        public int port = 26950;
        public TcpConnectionToServer TcpConnectionToServer;
        public UdpConnectionToServer UdpConnectionToServer;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Debug.Log("Instance already exists, destroying the object!");
                Destroy(this);
            }
        }

        private void Start()
        {
            TcpConnectionToServer = new TcpConnectionToServer();
            UdpConnectionToServer = new UdpConnectionToServer();
        }

        public void ConnectToServer()
        {
            TcpConnectionToServer.Connect();
        }
    }
}
