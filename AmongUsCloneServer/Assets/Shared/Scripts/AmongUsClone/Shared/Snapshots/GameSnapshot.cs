// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace AmongUsClone.Shared.Snapshots
{
    /**
     * Snapshot with an information about every object for each player
     */
    public class GameSnapshot
    {
        public readonly int id;
        public Dictionary<int, SnapshotPlayerInfo> playersInfo;

        public bool securityCamerasEnabled;

        public bool gameStarts;
        public bool gameStarted;

        public int impostorsAmount;

        public GameSnapshot(int id, Dictionary<int, SnapshotPlayerInfo> playersInfo, bool gameStarts, bool gameStarted, int impostorsAmount, bool securityCamerasEnabled)
        {
            // Shared
            this.id = id;
            this.playersInfo = playersInfo;

            // Lobby game phase
            this.gameStarts = gameStarts;
            this.gameStarted = gameStarted;

            // Play game phase
            this.impostorsAmount = impostorsAmount;
            this.securityCamerasEnabled = securityCamerasEnabled;
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
