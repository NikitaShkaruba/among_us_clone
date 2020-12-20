using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Snapshots;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Snapshots
{
    public static class GameSnapshots
    {
        private static readonly List<GameSnapshot> gameSnapshots = new List<GameSnapshot>();

        public static void ProcessSnapshot()
        {
            GameSnapshot lastGameSnapshot = CaptureSnapshot();

            CacheSnapshot(lastGameSnapshot);

            if (Server.clients.Count != 0)
            {
                PacketsSender.SendGameSnapshotPackets(lastGameSnapshot);
            }
        }

        private static void CacheSnapshot(GameSnapshot lastGameSnapshot)
        {
            gameSnapshots.Add(lastGameSnapshot);

            if (gameSnapshots.Count > 1000)
            {
                gameSnapshots.RemoveAt(0);
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Game snapshot #{lastGameSnapshot.id} captured");
        }

        private static GameSnapshot CaptureSnapshot()
        {
            int gameSnapshotId = gameSnapshots.Count == 0 ? 0 : gameSnapshots.Last().id + 1;
            List<Player> players = CaptureSnapshotPlayers().ToList();

            return new GameSnapshot(gameSnapshotId, players);
        }

        private static IEnumerable<Player> CaptureSnapshotPlayers()
        {
            List<Player> snapshotPlayers = new List<Player>();

            if (Server.clients.Count == 0)
            {
                return snapshotPlayers;
            }

            foreach (Client client in Server.clients.Values)
            {
                if (!client.IsPlayerInitialized())
                {
                    continue;
                }

                snapshotPlayers.Add(client.player);
            }

            return snapshotPlayers;
        }
    }
}
