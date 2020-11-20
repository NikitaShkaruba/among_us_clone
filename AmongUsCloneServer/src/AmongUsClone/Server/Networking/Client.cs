using AmongUsClone.Shared.DataStructures;

namespace AmongUsClone.Server.Networking
{
    public class Client
    {
        public readonly int Id;
        public Player Player;
        
        public readonly TcpConnectionToClient TcpConnectionToClient;
        public readonly UdpConnectionToClient UdpConnectionToClient;

        public Client(int id)
        {
            Id = id;
            
            TcpConnectionToClient = new TcpConnectionToClient(id);
            UdpConnectionToClient = new UdpConnectionToClient(id);
        }

        public void SendIntoGame(string playerName)
        {
            Player = new Player(Id, playerName, new Vector2(0, 0));

            foreach (Client client in Server.Clients.Values)
            {
                // Because of a multithreading we should check for it
                if (client.Player == null)
                {
                    continue;
                }
                
                // Spawn players (including himself) at each client
                PacketsSender.SendSpawnPlayerPacket(client.Id, Player);
                
                // Spawn new player at each client (himself is already spawned)
                if (client.Id != Id)
                {
                    PacketsSender.SendSpawnPlayerPacket(Id, client.Player);
                }
            }
        }
    }
}
