namespace AmongUsClone.Server.Networking
{
    public class Client
    {
        public int id;
        public readonly TcpConnection TcpConnection;

        public Client(int id)
        {
            this.id = id;
            TcpConnection = new TcpConnection(id);
        }
    }
}
