// ReSharper disable UnusedMember.Global

using UnityEngine;

namespace AmongUsClone.Shared.Snapshots
{
    public class SnapshotPlayerInfo
    {
        public int id;
        public Vector2 position;

        public SnapshotPlayerInfo(int id, Vector2 position)
        {
            this.id = id;
            this.position = position;
        }
    }
}
