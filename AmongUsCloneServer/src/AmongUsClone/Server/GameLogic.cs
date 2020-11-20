using AmongUsClone.Server.Networking;
using AmongUsClone.Shared;

namespace AmongUsClone.Server
{
    public static class GameLogic
    {
        public static void Update()
        {
            // We need to lock the collection, because in multi-thread environment it can be modified while being iterated
            lock (Server.Clients)
            {
                foreach (Client client in Server.Clients.Values)
                {
                    client.player?.Update();
                }
            }

            ThreadManager.UpdateMain();
        }
    }
}
