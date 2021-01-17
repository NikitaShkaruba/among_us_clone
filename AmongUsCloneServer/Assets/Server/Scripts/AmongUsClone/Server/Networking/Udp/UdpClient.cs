using System;
using System.Net;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Networking;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Networking.Udp
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "UdpClient", menuName = "ScriptableObjects/UdpClient")]
    public class UdpClient : ScriptableObject
    {
        [SerializeField] private Game.PlayersManager playersManager;
        [SerializeField] private PacketsReceiver packetsReceiver;

        System.Net.Sockets.UdpClient udpClient;

        public void Initialize(int port)
        {
            udpClient = new System.Net.Sockets.UdpClient(port);
            udpClient.BeginReceive(OnConnection, null);
        }

        public void SendPacket(Packet packet, IPEndPoint ipEndPoint)
        {
            try
            {
                udpClient.BeginSend(packet.ToArray(), packet.GetLength(), ipEndPoint, null, null);
            }
            catch (Exception exception)
            {
                Logger.LogError(LoggerSection.Network, $"Error sending UDP data to {ipEndPoint}: {exception}");
            }
        }

        private void OnConnection(IAsyncResult result)
        {
            try
            {
                IPEndPoint clientIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.EndReceive(result, ref clientIpEndPoint);

                // Start listening for the next client connection
                udpClient.BeginReceive(OnConnection, null);

                if (data.Length < sizeof(int))
                {
                    return;
                }

                Packet packet = new Packet(data);
                int playerId = packet.ReadInt();

                if (!playersManager.clients.ContainsKey(playerId))
                {
                    Logger.LogNotice(LoggerSection.Network, $"Skipping tcp packed from player {playerId}, because it is already disconnected");
                    return;
                }

                if (!playersManager.clients[playerId].IsConnectedViaUdp())
                {
                    playersManager.clients[playerId].ConnectUdp(clientIpEndPoint);
                    return;
                }

                if (!playersManager.clients[playerId].IsCorrectUdpIpEndPoint(clientIpEndPoint))
                {
                    Logger.LogError(LoggerSection.Network, "Hacking attempt, client ids doesn't match");
                    return;
                }

                HandlePacketData(playerId, packet);
            }
            catch (ObjectDisposedException objectDisposedException)
            {
                Logger.LogNotice(LoggerSection.Network, $"Error receiving UDP data because udpClient is already disposed: {objectDisposedException}");
            }
            catch (Exception exception)
            {
                Logger.LogError(LoggerSection.Network, $"Error receiving UDP data: {exception}");
            }
        }

        private void HandlePacketData(int playerId, Packet packet)
        {
            int packetLength = packet.ReadInt();
            byte[] packetBytes = packet.ReadBytes(packetLength);

            MainThread.ScheduleExecution(() =>
            {
                Packet newPacket = new Packet(packetBytes);
                int packetTypeId = newPacket.ReadInt();

                packetsReceiver.ProcessPacket(playerId, packetTypeId, newPacket, false);
            });
        }

        public void StopListening()
        {
            udpClient.Close();
        }
    }
}
