using AmongUsClone.Shared.DataStructures;

namespace AmongUsClone.Server.Networking
{
    public class Client
    {
        public readonly int id;
        public Player player;
        
        public readonly TcpConnectionToClient tcpConnectionToClient;
        public readonly UdpConnectionToClient udpConnectionToClient;

        public Client(int id)
        {
            this.id = id;
            
            tcpConnectionToClient = new TcpConnectionToClient(id);
            udpConnectionToClient = new UdpConnectionToClient(id);
        }

        public void SendIntoGame(string playerName)
        {
            player = new Player(id, playerName, new Vector2(0, 0));

            foreach (Client client in Server.Clients.Values)
            {
                // Because of a multithreading we should check for it
                if (client.player == null)
                {
                    continue;
                }
                
                // Connect existent players with the new client (including himself)
                PacketsSender.SendPlayerConnectedPacket(client.id, player);
                
                // Connect new player with each client (himself is already spawned)
                if (client.id != id)
                {
                    PacketsSender.SendPlayerConnectedPacket(id, client.player);
                }
            }
        }
    }
}
