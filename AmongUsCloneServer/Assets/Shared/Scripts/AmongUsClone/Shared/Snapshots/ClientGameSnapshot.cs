// ReSharper disable UnusedMember.Global

using System.Collections.Generic;

namespace AmongUsClone.Shared.Snapshots
{
    public class ClientGameSnapshot : GameSnapshot
    {
        public readonly int yourLastProcessedInputId;

        public ClientGameSnapshot(GameSnapshot prototype, int yourLastProcessedInputId) : base(prototype.id, prototype.playersInfo)
        {
            this.yourLastProcessedInputId = yourLastProcessedInputId;
        }

        public ClientGameSnapshot(int id, int yourLastProcessedInputId, List<SnapshotPlayerInfo> snapshotPlayersInfoInfo) : base(id, snapshotPlayersInfoInfo)
        {
            this.yourLastProcessedInputId = yourLastProcessedInputId;
        }
    }
}
