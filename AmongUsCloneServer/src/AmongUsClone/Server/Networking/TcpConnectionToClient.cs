using System;
using System.Net;
using System.Net.Sockets;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Server.Networking
{
    public class TcpConnectionToClient : TcpConnection
    {
        private static TcpListener tcpListener;
        private readonly int clientId;

        public TcpConnectionToClient(int clientId)
        {
            this.clientId = clientId;
        }

        public static void InitializeListener(int port)
        {
            tcpListener = new TcpListener(IPAddress.Any, port);
            tcpListener.Start();
            tcpListener.BeginAcceptTcpClient(OnConnection, null);
        }

        private void Connect(TcpClient tcpClient)
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

        private static void OnConnection(IAsyncResult result)
        {
            TcpClient tcpClient = tcpListener.EndAcceptTcpClient(result);
            Logger.LogEvent($"Incoming tcp connection from {tcpClient.Client.RemoteEndPoint}...");

            // Start listening for the next client connection
            tcpListener.BeginAcceptTcpClient(OnConnection, null);

            for (int clientId = Server.MinPlayerId; clientId <= Server.MaxPlayerId; clientId++)
            {
                // If this client exists already - skip this clientId in order to find a free one
                if (Server.Clients.ContainsKey(clientId))
                {
                    continue;
                }

                Server.Clients[clientId] = new Client(clientId);
                Server.Clients[clientId].TcpConnectionToClient.Connect(tcpClient);
                return;
            }

            Logger.LogError($"{tcpClient.Client.RemoteEndPoint} failed to connect a client: Server is full");
        }
    }
}
