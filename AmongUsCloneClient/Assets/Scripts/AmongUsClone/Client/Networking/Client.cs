using UnityEngine;

namespace AmongUsClone.Client.Networking
{
    public class Client : MonoBehaviour
    {
        public static Client Instance;

        public int id = 0;
        public string ip = "127.0.0.1";
        public int port = 26950;
        private TcpConnection tcpConnection;

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
            tcpConnection = new TcpConnection();
        }

        public void ConnectToServer()
        {
            tcpConnection.Connect();
        }
    }
}
