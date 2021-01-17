using System;
using System.Net.Sockets;
using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Server.Networking.Tcp
{
    public class TcpConnection : Shared.Networking.TcpConnection
    {
        private PacketsReceiver packetsReceiver;
        private PacketsSender packetsSender;
        private LobbyGamePhase lobbyGamePhase;
        private readonly int playerId;

        public TcpConnection(int playerId, TcpClient tcpClient, PacketsReceiver packetsReceiver, PacketsSender packetsSender, LobbyGamePhase lobbyGamePhase)
        {
            this.playerId = playerId;
            receivePacket = new Packet();
            receiveBuffer = new byte[DataBufferSize];

            this.packetsReceiver = packetsReceiver;
            this.packetsSender = packetsSender;
            this.lobbyGamePhase = lobbyGamePhase;

            this.tcpClient = tcpClient;
            this.tcpClient.ReceiveBufferSize = DataBufferSize;
            this.tcpClient.SendBufferSize = DataBufferSize;
        }

        public void InitializeCommunication()
        {
            stream = tcpClient.GetStream();
            stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);

            packetsSender.SendWelcomePacket(playerId);
        }

        private void ReceiveDataCallback(IAsyncResult result)
        {
            try
            {
                int byteLength = stream.EndRead(result);
                if (byteLength <= 0)
                {
                    MainThread.ScheduleExecution(() => lobbyGamePhase.DisconnectPlayer(playerId));
                    return;
                }

                byte[] data = new byte[byteLength];
                Array.Copy(receiveBuffer, data, byteLength);

                bool shouldReset = HandleData(data, (packetTypeId, packet) => packetsReceiver.ProcessPacket(playerId, packetTypeId, packet, true));
                receivePacket.Reset(shouldReset);

                stream.BeginRead(receiveBuffer, 0, DataBufferSize, ReceiveDataCallback, null);
            }
            catch (Exception exception)
            {
                Logger.LogError(LoggerSection.Network, $"Error receiving TCP data: {exception}");
                MainThread.ScheduleExecution(() => lobbyGamePhase.DisconnectPlayer(playerId));
            }
        }
    }
}
