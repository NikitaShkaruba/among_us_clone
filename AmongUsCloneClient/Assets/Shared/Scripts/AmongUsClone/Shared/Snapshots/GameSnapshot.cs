// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Shared.Game.PlayerLogic;

namespace AmongUsClone.Shared.Snapshots
{
    /**
     * Snapshot with an information about every object for each player
     * Todo: consider migrating yourLastProcessedInputId to separate class
     */
    public class GameSnapshot
    {
        public readonly int id;
        public readonly int yourLastProcessedInputId;

        public readonly List<SnapshotPlayerInfo> playersInfo;

        public GameSnapshot(int id, List<SnapshotPlayerInfo> snapshotPlayersInfoInfo)
        {
            this.id = id;
            yourLastProcessedInputId = 0;
            playersInfo = snapshotPlayersInfoInfo;
        }

        public GameSnapshot(GameSnapshot prototype, int yourLastProcessedInputId)
        {
            id = prototype.id;
            playersInfo = prototype.playersInfo;
            this.yourLastProcessedInputId = yourLastProcessedInputId;
        }

        public GameSnapshot(int id, int yourLastProcessedInputId, List<SnapshotPlayerInfo> snapshotPlayersInfoInfo)
        {
            this.id = id;
            this.yourLastProcessedInputId = yourLastProcessedInputId;
            playersInfo = snapshotPlayersInfoInfo;
        }

        public GameSnapshot(int id, IEnumerable<Player> players)
        {
            this.id = id;
            yourLastProcessedInputId = 0;
            playersInfo = players.Select(player => new SnapshotPlayerInfo(player.id, player.Position)).ToList();
        }
    }
}
