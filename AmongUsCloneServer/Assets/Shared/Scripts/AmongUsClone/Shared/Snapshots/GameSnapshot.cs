// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Shared.Game.PlayerLogic;

namespace AmongUsClone.Shared.Snapshots
{
    /**
     * Snapshot with an information about every object for each player
     */
    public readonly struct GameSnapshot
    {
        public readonly int id;
        public readonly List<SnapshotPlayerInfo> playersInfo;

        public GameSnapshot(int id, List<SnapshotPlayerInfo> snapshotPlayersInfoInfo)
        {
            this.id = id;
            playersInfo = snapshotPlayersInfoInfo;
        }

        public GameSnapshot(int id, IEnumerable<Player> players)
        {
            this.id = id;
            playersInfo = players.Select(player => new SnapshotPlayerInfo(player.id, player.Position)).ToList();
        }
    }
}
