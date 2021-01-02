// ReSharper disable UnusedMember.Global

using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Shared.Game.PlayerLogic;

namespace AmongUsClone.Shared.Snapshots
{
    /**
     * Snapshot with an information about every object for each player
     */
    public class GameSnapshot
    {
        public readonly int id;
        public readonly Dictionary<int, SnapshotPlayerInfo> playersInfo;

        public GameSnapshot(int id, Dictionary<int, SnapshotPlayerInfo> snapshotPlayersInfoInfo)
        {
            this.id = id;
            playersInfo = snapshotPlayersInfoInfo;
        }

        public GameSnapshot(int id, IEnumerable<PlayerInformation> players)
        {
            this.id = id;
            playersInfo = players.Select(playerInformation => new SnapshotPlayerInfo(playerInformation.id, playerInformation.transform.position)).ToDictionary(snapshotPlayerInfo => snapshotPlayerInfo.id);
        }

        public override string ToString()
        {
            List<string> playersInfoDescriptionPieces = new List<string>(playersInfo.Count);
            foreach (SnapshotPlayerInfo playerInfo in playersInfo.Values)
            {
                playersInfoDescriptionPieces.Add($"{{ id: {playerInfo.id}, position: {playerInfo.position} }}");
            }

            string playersInfoDescription = string.Join(",", playersInfoDescriptionPieces);

            return $"#{id}. Players: {{ {playersInfoDescription} }}";
        }
    }
}
