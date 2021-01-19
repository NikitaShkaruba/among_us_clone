using System;
using System.Net.Sockets;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Client.Networking
{
    public class TcpConnection : Shared.Networking.TcpConnection
    {
        private readonly PacketsReceiver packetsReceiver;
        private readonly ConnectionToServer connectionToServer;

        public TcpConnection(ConnectionToServer connectionToServer, PacketsReceiver packetsReceiver, MetaMonoBehaviours metaMonoBehaviours) : base(metaMonoBehaviours)
        {
            this.connectionToServer = connectionToServer;
            this.packetsReceiver = packetsReceiver;

            tcpClient = new TcpClient
            {
                ReceiveBufferSize = DataBufferSize,
                SendBufferSize = DataBufferSize
            };

            receiveBuffer = new byte[DataBufferSize];
            tcpClient.BeginConnect(ConnectionToServer.ServerIP, ConnectionToServer.ServerPort, ConnectCallback, tcpClient);
            Logger.LogEvent(LoggerSection.Network, "Started listening for tcp connections");
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
                    connectionToServer.Disconnect();
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);

                bool hasReadFullPacket = HandleData(data, (packetTypeId, packet) => packetsReceiver.ProcessPacket(packetTypeId, packet, true));
                receivePacket.Reset(hasReadFullPacket);

                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);
            }
            catch
            {
                connectionToServer.Disconnect();
            }
        }
    }
}
