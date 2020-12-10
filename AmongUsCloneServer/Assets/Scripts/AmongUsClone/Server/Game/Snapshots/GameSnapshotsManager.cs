using Logger = AmongUsClone.Shared.Logging.Logger;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Logging;
using UnityEngine;

namespace AmongUsClone.Server.Game.Snapshots
{
    public static class GameSnapshots
    {
        private static readonly List<GameSnapshot> gameSnapshots = new List<GameSnapshot>();

        public static IEnumerator ProcessCurrentGameSnapshot()
        {
            yield return new WaitForFixedUpdate();

            GameSnapshot lastGameSnapshot = CaptureGameSnapshot();

            SaveGameSnapshot(lastGameSnapshot);

            if (Server.clients.Count != 0)
            {
                PacketsSender.SendGameSnapshotPackets(lastGameSnapshot);
            }
        }

        private static void SaveGameSnapshot(GameSnapshot lastGameSnapshot)
        {
            gameSnapshots.Add(lastGameSnapshot);

            if (gameSnapshots.Count > 1000)
            {
                gameSnapshots.RemoveAt(0);
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Game snapshot #{lastGameSnapshot.id} captured");
        }

        private static GameSnapshot CaptureGameSnapshot()
        {
            int gameSnapshotId = gameSnapshots.Count == 0 ? 0 : gameSnapshots.Last().id + 1;
            List<Player> players = GetPlayersForSnapshot();

            return new GameSnapshot(gameSnapshotId, players);
        }

        private static List<Player> GetPlayersForSnapshot()
        {
            List<Player> snapshotPlayers = new List<Player>();

            if (Server.clients.Count == 0)
            {
                return snapshotPlayers;
            }

            foreach (Client client in Server.clients.Values)
            {
                snapshotPlayers.Add(client.player);
            }

            return snapshotPlayers;
        }
    }
}
