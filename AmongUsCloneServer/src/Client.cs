namespace AmongUsCloneServer
{
    public class Client
    {
        public int id;
        public Tcp tcp;

        public Client(int id)
        {
            this.id = id;
            tcp = new Tcp(id);
        }
    }
}
