using System;
using System.Net.Sockets;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Client.Networking
{
    public class TcpConnectionToServer : TcpConnection
    {
        public void Connect()
        {
            TcpClient = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };

            ReceiveBuffer = new byte[DataBufferSize];
            TcpClient.BeginConnect(Client.Instance.ip, Client.Instance.port, ConnectCallback, TcpClient);
        }

        private void ConnectCallback(IAsyncResult result)
        {
            TcpClient.EndConnect(result);

            if (!TcpClient.Connected)
            {
                return;
            }

            Stream = TcpClient.GetStream();

            ReceivePacket = new Packet();

            Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);
        }

        private void ReceiveDataCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = Stream.EndRead(result);
                if (byteLength <= 0)
                {
                    // Todo: disconnect
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(ReceiveBuffer, data, byteLength);

                bool hasReadFullPacket = HandleData(data, PacketsReceiver.ProcessPacket);
                ReceivePacket.Reset(hasReadFullPacket);

                Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);
            }
            catch
            {
                // Todo: disconnect
            }
        }
    }
}
