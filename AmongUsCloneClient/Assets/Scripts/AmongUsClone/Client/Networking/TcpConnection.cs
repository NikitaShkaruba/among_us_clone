using System;
using System.Net.Sockets;

namespace AmongUsClone.Client.Networking
{
    public class TcpConnection
    {
        private const int DataBufferSize = 4096;

        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] receiveBuffer;

        public void Connect()
        {
            tcpClient = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };

            receiveBuffer = new byte[DataBufferSize];
            tcpClient.BeginConnect(Client.Instance.ip, Client.Instance.port, ConnectCallback, tcpClient);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            tcpClient.EndConnect(result);

            if (!tcpClient.Connected)
            {
                return;
            }

            stream = tcpClient.GetStream();

            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);
        }

        private void ReceiveDataCallback(IAsyncResult result)
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

                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);
            }
            catch
            {
                // Todo: disconnect
            }
        }
    }
}