using AmongUsClone.Server.Networking;
using AmongUsClone.Shared;

namespace AmongUsClone.Server
{
    public static class GameLogic
    {
        public static void Update()
        {
            foreach (Client client in Server.Clients.Values)
            {
                client.player?.Update();
            }
            
            ThreadManager.UpdateMain();
        }
    }
}
