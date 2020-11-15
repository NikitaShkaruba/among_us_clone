using System;
using System.Net;
using System.Net.Sockets;
using AmongUsClone.Server.Infrastructure;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Networking;

namespace AmongUsClone.Server.Networking
{
    public class UdpConnectionToClient
    {
        private readonly int clientId;
        
        private static UdpClient udpClient;
        private IPEndPoint ipEndPoint;

        public UdpConnectionToClient(int clientId)
        {
            this.clientId = clientId;
        }

        public static void InitializeListener(int port)
        {
            udpClient = new UdpClient(port);
            udpClient.BeginReceive(OnConnection, null);
        }

        public void SendPacket(Packet packet)
        {
            if (ipEndPoint == null)
            {
                throw new Exception("Undefined clientIpEndPoint");
            }

            try
            {
                udpClient.BeginSend(packet.ToArray(), packet.GetLength(), ipEndPoint, null, null);
            }
            catch (Exception exception)
            {
                Logger.LogError($"Error sending data to {ipEndPoint} via UDP: {exception}");
            }
        }
 
        private void Connect(IPEndPoint ipEndPoint)
        {
            this.ipEndPoint = ipEndPoint;
            PacketsSender.SendUdpTestPacket(clientId);
        }

        private static void OnConnection(IAsyncResult result)
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

                using (Packet packet = new Packet(data))
                {
                    int clientId = packet.ReadInt();

                    if (clientId == 0)
                    {
                        Logger.LogError("Undefined client id in OnUdpConnection");
                        return;
                    }

                    if (IsUnknownClient(clientId))
                    {
                        Server.Clients[clientId].UdpConnectionToClient.Connect(clientIpEndPoint);
                    }
                    else
                    {
                        if (Server.Clients[clientId].UdpConnectionToClient.ipEndPoint.ToString() != clientIpEndPoint.ToString())
                        {
                            Logger.LogError("Hack attempt, client ids doesn't match");
                            return;
                        }

                        Server.Clients[clientId].UdpConnectionToClient.HandlePacketData(packet);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.LogError($"Error receiving Udp data: {exception}");
            }
        }

        private void HandlePacketData(Packet packet)
        {
            int packetLength = packet.ReadInt();
            byte[] packetBytes = packet.ReadBytes(packetLength);
            
            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet newPacket = new Packet(packetBytes))
                {
                    int packetTypeId = newPacket.ReadInt();
                    PacketsReceiver.ProcessPacket(clientId, packetTypeId, newPacket);
                }
            });
        }
        
        private static bool IsUnknownClient(int clientId)
        {
            return Server.Clients[clientId].UdpConnectionToClient.ipEndPoint == null;
        }

    }
}
