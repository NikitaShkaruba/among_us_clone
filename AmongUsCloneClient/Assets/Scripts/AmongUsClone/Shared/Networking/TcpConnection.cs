using System;
using System.Net.Sockets;

namespace AmongUsClone.Shared.Networking
{
    /**
     * Class that encapsulates a connection via TCP
     */
    public class TcpConnection
    {
        protected const int DataBufferSize = 4096;

        public TcpClient TcpClient;

        protected NetworkStream Stream;
        protected Packet ReceivePacket;
        protected byte[] ReceiveBuffer;

        // Todo: move packetTypeId computation into Packet
        public delegate void OnPacketReceivedCallback(int packetTypeId, Packet packet);

        /**
         * Send data to a connected TcpClient
         */
        public void SendPacket(Packet packet)
        {
            if (TcpClient == null)
            {
                throw new Exception("TcpConnection has no tcpClient");
            }

            try
            {
                Stream.BeginWrite(packet.ToArray(), 0, packet.GetLength(), null, null);
            }
            catch (Exception exception)
            {
                throw new Exception($"Unable to send tcp data: ${exception}");
            }
        }
        
        /**
         * TCP protocol is stream based - that means that we don't always have exactly 1 packet there - we just have
         * random bytes which can include half a packet, one packet, one and a half packet, or even more.
         * Because of it we need co compose our packets from those bytes by hand, and when we got ourselves a full packet - process it with HandlePacketCallback
         *
         * @returns - if full packet reset is needed
         */
        protected bool HandleData(byte[] data, OnPacketReceivedCallback onPacketReceivedCallback)
        {
            int packetLength = 0;
            
            ReceivePacket.WriteBytesAndPrepareToRead(data);

            // If start of the packet is (DataPacketTypeId) - read it and let try to create a packet to handle
            if (ReceivePacket.GetUnreadLength() >= sizeof(int))
            {
                packetLength = ReceivePacket.ReadInt();
                if (packetLength <= 0)
                {
                    return true;
                }
            }

            // If we have a packet to handle - handle it on main thread
            while (packetLength > 0 && packetLength <= ReceivePacket.GetUnreadLength())
            {
                byte[] packetBytes = ReceivePacket.ReadBytes(packetLength);
                
                ThreadManager.ExecuteOnMainThread(() =>
                {
                    using (Packet packet = new Packet(packetBytes))
                    {
                        int packetTypeId = packet.ReadInt();
                        onPacketReceivedCallback(packetTypeId, packet);
                    }
                });

                packetLength = 0;
                
                // If we still have a packet
                if (ReceivePacket.GetUnreadLength() >= sizeof(int))
                {
                    packetLength = ReceivePacket.ReadInt();
                    if (packetLength <= 0)
                    {
                        return true;
                    }
                }
            }

            // Todo: understand why... This is most probably a hotfix of some kind
            if (packetLength <= 1)
            {
                return true;
            }

            // We got ourselves only a part of a full packet - so we need to wait for it's next parts
            return false;
        }
    }
}
