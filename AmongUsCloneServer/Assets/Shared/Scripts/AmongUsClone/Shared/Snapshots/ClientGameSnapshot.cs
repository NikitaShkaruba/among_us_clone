// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace AmongUsClone.Shared.Snapshots
{
    public class ClientGameSnapshot : GameSnapshot
    {
        public readonly int yourLastProcessedInputId;
        public readonly Dictionary<int, int> adminPanelPositions;

        public ClientGameSnapshot(GameSnapshot prototype, int yourLastProcessedInputId, Dictionary<int, int> adminPanelPositions) : base(prototype.id, prototype.playersInfo, prototype.gameStarts, prototype.gameStarted, prototype.impostorsAmount, prototype.securityCamerasEnabled)
        {
            this.yourLastProcessedInputId = yourLastProcessedInputId;
            this.adminPanelPositions = adminPanelPositions;
        }

        public ClientGameSnapshot(int id, int yourLastProcessedInputId, Dictionary<int, SnapshotPlayerInfo> playersInfo, bool gameStarts, bool gameStarted, int impostorsAmount, Dictionary<int, int> adminPanelPositions, bool securityCamerasEnabled) : base(id, playersInfo, gameStarts, gameStarted, impostorsAmount, securityCamerasEnabled)
        {
            this.yourLastProcessedInputId = yourLastProcessedInputId;
            this.adminPanelPositions = adminPanelPositions;
        }
    }
}
