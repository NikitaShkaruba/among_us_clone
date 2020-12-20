// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Shared.Game.PlayerLogic;

namespace AmongUsClone.Shared.Snapshots
{
    /**
     * Snapshot with an information about every object for each player
     * Todo: consider migrating lastControlsRequestId to separate class
     */
    public struct GameSnapshot
    {
        public readonly int id;
        public readonly int lastControlsRequestId;

        public readonly List<SnapshotPlayerInfo> playersInfo;

        public GameSnapshot(int id, List<SnapshotPlayerInfo> snapshotPlayersInfoInfo)
        {
            this.id = id;
            lastControlsRequestId = 0;
            playersInfo = snapshotPlayersInfoInfo;
        }

        public GameSnapshot(GameSnapshot prototype, int lastControlsRequestId)
        {
            id = prototype.id;
            playersInfo = prototype.playersInfo;
            this.lastControlsRequestId = lastControlsRequestId;
        }

        public GameSnapshot(int id, int lastControlsRequestId, List<SnapshotPlayerInfo> snapshotPlayersInfoInfo)
        {
            this.id = id;
            this.lastControlsRequestId = lastControlsRequestId;
            playersInfo = snapshotPlayersInfoInfo;
        }

        public GameSnapshot(int id, IEnumerable<Player> players)
        {
            this.id = id;
            lastControlsRequestId = 0;
            playersInfo = players.Select(player => new SnapshotPlayerInfo(player.id, player.Position)).ToList();
        }
    }
}
