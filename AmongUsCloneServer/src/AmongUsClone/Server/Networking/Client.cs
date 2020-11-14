namespace AmongUsClone.Server.Networking
{
    public class Client
    {
        public int id;
        public readonly TcpConnectionToClient TcpConnection;

        public Client(int id)
        {
            this.id = id;
            TcpConnection = new TcpConnectionToClient(id);
        }
    }
}
