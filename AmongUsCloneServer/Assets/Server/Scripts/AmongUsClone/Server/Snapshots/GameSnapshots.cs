using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Snapshots;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Snapshots
{
    public static class GameSnapshots
    {
        private static readonly Dictionary<int, GameSnapshot> gameSnapshots = new Dictionary<int, GameSnapshot>();

        public static void ProcessSnapshot()
        {
            GameSnapshot lastGameSnapshot = CaptureSnapshot();

            SaveSnapshot(lastGameSnapshot);

            if (Server.clients.Count != 0)
            {
                PacketsSender.SendGameSnapshotPackets(lastGameSnapshot);
            }
        }

        private static void SaveSnapshot(GameSnapshot gameSnapshot)
        {
            gameSnapshots.Add(gameSnapshot.id, gameSnapshot);

            if (gameSnapshots.Count > 1000)
            {
                gameSnapshots.Remove(gameSnapshots.Keys.Min());
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Game snapshot captured {gameSnapshot}");
        }

        private static GameSnapshot CaptureSnapshot()
        {
            int snapshotId = gameSnapshots.Count != 0 ? gameSnapshots.Keys.Max() + 1 : 0;
            List<Player> players = CaptureSnapshotPlayers().ToList();

            return new GameSnapshot(snapshotId, players);
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
