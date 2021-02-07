// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace AmongUsClone.Shared.Snapshots
{
    public class ClientGameSnapshot : GameSnapshot
    {
        public readonly int yourLastProcessedInputId;

        public Dictionary<int, int> adminPanelPositions;

        public ClientGameSnapshot(GameSnapshot prototype, int yourLastProcessedInputId, Dictionary<int, int> adminPanelPositions) : base(prototype.id, prototype.playersInfo)
        {
            this.yourLastProcessedInputId = yourLastProcessedInputId;
            this.adminPanelPositions = adminPanelPositions;
        }

        public ClientGameSnapshot(int id, int yourLastProcessedInputId, Dictionary<int, SnapshotPlayerInfo> snapshotPlayersInfo, Dictionary<int, int> adminPanelPositions) : base(id, snapshotPlayersInfo)
        {
            this.yourLastProcessedInputId = yourLastProcessedInputId;
            this.adminPanelPositions = adminPanelPositions;
        }
    }
}
