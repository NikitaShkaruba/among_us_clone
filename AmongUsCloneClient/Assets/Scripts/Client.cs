using System;
using System.Net.Sockets;
using UnityEngine;

public class Client : MonoBehaviour
{
    public static Client instance;
    private const int DataBufferSize = 4096;

    public string ip = "127.0.0.1";
    public int port = 26950;
    public int id = 0;
    private Tcp tcp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying the object!");
            Destroy(this);
        }
    }

    private void Start()
    {
        tcp = new Tcp();
    }

    public void ConnectToServer()
    {
        tcp.Connect();
    }

    public class Tcp
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer ;

        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };

            receiveBuffer = new byte[DataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            socket.EndConnect(result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    // Todo: disconnect
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);

                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch
            {
                // Todo: disconnect
            }
        }
    }
}
