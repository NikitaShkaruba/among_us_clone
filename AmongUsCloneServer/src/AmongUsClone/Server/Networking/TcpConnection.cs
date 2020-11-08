using System;
using System.Net.Sockets;
using AmongUsClone.Server.Infrastructure;

namespace AmongUsClone.Server.Networking
{
    public class TcpConnection
    {
        private const int DataBufferSize = 4096;

        private readonly int clientId;
        private TcpClient tcpClient;
        
        private byte[] receiveBuffer;
        private NetworkStream stream;

        public TcpConnection(int clientId)
        {
            this.clientId = clientId;
        }

        public void Connect(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;
            this.tcpClient.ReceiveBufferSize = DataBufferSize;
            this.tcpClient.SendBufferSize = DataBufferSize;

            receiveBuffer = new byte[DataBufferSize];
            
            stream = tcpClient.GetStream();
            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);

            // Todo: send welcome packet
        }

        private void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    // Todo: disconnect the client
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);

                // Todo: handle data
                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveCallback, null);
            }
            catch (Exception exception)
            {
                Logger.LogError($"Error receiving TCP data: {exception}");
            }
        }
    }
}
