using System;
using System.Net.Sockets;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Server.Networking
{
    public class TcpConnectionToClient: TcpConnection
    {
        private readonly int clientId;

        public TcpConnectionToClient(int clientId)
        {
            this.clientId = clientId;
        }

        public void Connect(TcpClient tcpClient)
        {
            TcpClient = tcpClient;
            TcpClient.ReceiveBufferSize = DataBufferSize;
            TcpClient.SendBufferSize = DataBufferSize;

            ReceivePacket = new Packet();
            ReceiveBuffer = new byte[DataBufferSize];
            
            Stream = tcpClient.GetStream();
            Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);

            PacketsSender.SendWelcomePacket(clientId, "Welcome to the server!");
        }
       
        private void ReceiveDataCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = Stream.EndRead(result);
                if (byteLength <= 0)
                {
                    // Todo: disconnect the client
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(ReceiveBuffer, data, byteLength);

                bool shouldReset = HandleData(data, (packetTypeId, packet) => PacketsReceiver.ProcessPacket(clientId, packetTypeId, packet));
                ReceivePacket.Reset(shouldReset);
                
                Stream.BeginRead(ReceiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);
            }
            catch (Exception exception)
            {
                Logger.LogError($"Error receiving TCP data: {exception}");
            }
        }
        
        private void HandleDataCallback(int packetTypeId, Packet packet)
        {
            
        }
    }
}
