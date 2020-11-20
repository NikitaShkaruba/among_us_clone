using System;
using System.Net;
using System.Net.Sockets;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Networking;
using UnityEngine;

namespace AmongUsClone.Client.Networking
{
    public class UdpConnectionToServer
    {
        private UdpClient udpClient;
        private IPEndPoint ipEndPoint;

        public UdpConnectionToServer()
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(Client.instance.ip), Client.instance.port);
        }

        public void Connect(int localPort)
        {
            udpClient = new UdpClient(localPort);

            udpClient.Connect(ipEndPoint);
            udpClient.BeginReceive(OnConnection, null);

            using (Packet packet = new Packet())
            {
                SendPacket(packet);
            }
        }

        public void SendPacket(Packet packet)
        {
            if (udpClient == null)
            {
                throw new Exception("Uninitialized UdpClient");
            }

            try
            {
                packet.InsertInt(Client.instance.id);
                udpClient.BeginSend(packet.ToArray(), packet.GetLength(), null, null);
            }
            catch (Exception exception)
            {
                Debug.Log($"Error sending data through udp: {exception}");
            }
        }

        private void OnConnection(IAsyncResult result)
        {
            try
            {
                byte[] data = udpClient.EndReceive(result, ref ipEndPoint);

                // Start listening for next connection again
                udpClient.BeginReceive(OnConnection, null);

                if (data.Length < sizeof(int))
                {
                    // Todo: disconnect
                    return;
                }
                
                HandlePacketData(data);
            }
            catch
            {
                // Todo: disconnect
            }
        }

        private static void HandlePacketData(byte[] data)
        {
            using (Packet packet = new Packet(data))
            {
                int packetLength = packet.ReadInt();
                data = packet.ReadBytes(packetLength);
            }

            ThreadManager.ExecuteOnMainThread(() =>
            {
                using (Packet packet = new Packet(data))
                {
                    int packetTypeId = packet.ReadInt();
                    PacketsReceiver.ProcessPacket(packetTypeId, packet);
                }
            });
        }
    }
}
