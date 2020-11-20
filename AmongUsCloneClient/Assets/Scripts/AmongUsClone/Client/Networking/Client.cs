using UnityEngine;

namespace AmongUsClone.Client.Networking
{
    public class Client : MonoBehaviour
    {
        public static Client instance;

        [HideInInspector] public int id;
        
        public TcpConnectionToServer tcpConnectionToServer;
        public UdpConnectionToServer udpConnectionToServer;
        public string ip = "127.0.0.1";
        public int port = 26950;
        private bool isConnected;

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

        // Unity holds some data between running game instances, so we need to cleanup by hand
        private void OnApplicationQuit()
        {
            DisconnectFromServer();
        }

        public void ConnectToServer()
        {
            tcpConnectionToServer.Connect();
            isConnected = true;
        }

        public void DisconnectFromServer()
        {
            // This check and this variable is needed, because unity is not Closing instantly on Application.Quit();
            if (!isConnected)
            {
                return;
            }
            
            isConnected = false;
            tcpConnectionToServer.tcpClient.Close();
            udpConnectionToServer.udpClient.Close();
            
            Debug.Log("Disconnected from the server");
            
            Application.Quit();
        }
    }
}
