using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Game;
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
        private static readonly Dictionary<int, GameSnapshot> gameSnapshots = new Dictionary<int, GameSnapshot>();

        public static void ProcessSnapshot()
        {
            GameSnapshot lastGameSnapshot = CaptureSnapshot();

            // Todo: remove debug
            if (GameManager.instance.lobby.players.ContainsKey(0))
            {
                GameSnapshotsDebug.Log(new ClientGameSnapshot(lastGameSnapshot.id, -1, lastGameSnapshot.playersInfo), GameManager.instance.lobby.players[0]);
            }

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

            // Todo: add more descriptive logs
            Logger.LogEvent(LoggerSection.GameSnapshots, $"Game snapshot #{gameSnapshot.id} captured");
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
