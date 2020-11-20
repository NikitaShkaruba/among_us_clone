using System;
using System.Net.Sockets;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Client.Networking
{
    public class TcpConnectionToServer : TcpConnection
    {
        public void Connect()
        {
            tcpClient = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };

            receiveBuffer = new byte[DataBufferSize];
            tcpClient.BeginConnect(Client.instance.ip, Client.instance.port, ConnectCallback, tcpClient);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            tcpClient.EndConnect(result);

            if (!tcpClient.Connected)
            {
                return;
            }

            stream = tcpClient.GetStream();

            receivePacket = new Packet();

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

                bool hasReadFullPacket = HandleData(data, PacketsReceiver.ProcessPacket);
                receivePacket.Reset(hasReadFullPacket);

                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);
            }
            catch
            {
                // Todo: disconnect
            }
        }
    }
}
