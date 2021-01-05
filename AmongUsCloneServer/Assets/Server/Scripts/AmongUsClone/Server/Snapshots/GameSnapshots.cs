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
            Dictionary<int, SnapshotPlayerInfo> players = CaptureSnapshotPlayers();

            return new GameSnapshot(snapshotId, players);
        }

        private static Dictionary<int, SnapshotPlayerInfo> CaptureSnapshotPlayers()
        {
            Dictionary<int, SnapshotPlayerInfo> snapshotPlayers = new Dictionary<int, SnapshotPlayerInfo>();

            if (Server.clients.Count == 0)
            {
                return snapshotPlayers;
            }

            foreach (Client client in Server.clients.Values)
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                snapshotPlayers[client.player.information.id] = new SnapshotPlayerInfo(
                    client.player.information.id,
                    client.player.transform.position,
                    client.player.controllable.playerInput
                );
            }

            return snapshotPlayers;
        }
    }
}
