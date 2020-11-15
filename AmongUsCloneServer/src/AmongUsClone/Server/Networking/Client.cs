namespace AmongUsClone.Server.Networking
{
    public class Client
    {
        public int id;
        public readonly TcpConnectionToClient TcpConnectionToClient;
        public readonly UdpConnectionToClient UdpConnectionToClient;

        public Client(int id)
        {
            this.id = id;
            
            TcpConnectionToClient = new TcpConnectionToClient(id);
            UdpConnectionToClient = new UdpConnectionToClient(id);
        }
    }
}
