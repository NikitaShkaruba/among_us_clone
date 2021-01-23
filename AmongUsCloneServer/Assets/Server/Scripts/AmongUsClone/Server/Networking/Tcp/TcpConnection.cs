using System;
using System.Net.Sockets;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Server.Networking.Tcp
{
    public class TcpConnection : Shared.Networking.TcpConnection
    {
        private MetaMonoBehaviours metaMonoBehaviours;
        private PacketsReceiver packetsReceiver;
        private PacketsSender packetsSender;
        private PlayersManager playersManager;
        private readonly int playerId;

        public TcpConnection(int playerId, TcpClient tcpClient, PacketsReceiver packetsReceiver, PacketsSender packetsSender, PlayersManager playersManager, MetaMonoBehaviours metaMonoBehaviours) : base(metaMonoBehaviours)
        {
            this.playerId = playerId;
            receivePacket = new Packet();
            receiveBuffer = new byte[DataBufferSize];

            this.packetsReceiver = packetsReceiver;
            this.packetsSender = packetsSender;
            this.playersManager = playersManager;
            this.metaMonoBehaviours = metaMonoBehaviours;

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
                    metaMonoBehaviours.applicationCallbacks.ScheduleFixedUpdateAction(() => playersManager.DisconnectPlayer(playerId));
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
                metaMonoBehaviours.applicationCallbacks.ScheduleFixedUpdateAction(() => playersManager.DisconnectPlayer(playerId));
            }
        }
    }
}
