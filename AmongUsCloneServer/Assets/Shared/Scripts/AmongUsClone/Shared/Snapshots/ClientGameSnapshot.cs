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

        public ClientGameSnapshot(int id, Dictionary<int, SnapshotPlayerInfo> snapshotPlayerInfo, int yourLastProcessedInputId) : base(id, snapshotPlayerInfo)
        {
            this.yourLastProcessedInputId = yourLastProcessedInputId;
        }

        public ClientGameSnapshot(int id, int yourLastProcessedInputId, Dictionary<int, SnapshotPlayerInfo> snapshotPlayersInfo) : base(id, snapshotPlayersInfo)
        {
            this.yourLastProcessedInputId = yourLastProcessedInputId;
        }
    }
}
