using AmongUsClone.Shared.DataStructures;

namespace AmongUsClone.Server.Networking
{
    public class Client
    {
        public readonly int id;
        public Player Player;
        
        public readonly TcpConnectionToClient TcpConnectionToClient;
        public readonly UdpConnectionToClient UdpConnectionToClient;

        public Client(int id)
        {
            this.id = id;
            
            TcpConnectionToClient = new TcpConnectionToClient(id);
            UdpConnectionToClient = new UdpConnectionToClient(id);
        }

        public void SendIntoGame(string playerName)
        {
            Player = new Player(id, playerName, new Vector2(0, 0));

            foreach (Client client in Server.Clients.Values)
            {
                // Because of a multithreading we should check for it
                if (client.Player == null)
                {
                    continue;
                }
                
                // Spawn players (including himself) at each client
                PacketsSender.SendSpawnPlayerPacket(client.id, Player);
                
                // Spawn new player at each client (himself is already spawned)
                if (client.id != id)
                {
                    PacketsSender.SendSpawnPlayerPacket(id, client.Player);
                }
            }
        }
    }
}
