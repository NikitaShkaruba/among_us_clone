using System.Collections.Generic;
using AmongUsClone.Client.Game;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Snapshots;

namespace AmongUsClone.Client.Snapshots
{
    public static class GameSnapshotsManager
    {
        public static void ProcessGameSnapshot(in GameSnapshot gameSnapshot)
        {
            foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo)
            {
                GameManager.instance.UpdatePlayerPosition(snapshotPlayerInfo.id, snapshotPlayerInfo.position);
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {gameSnapshot.id}");
        }
    }
}
