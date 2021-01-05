// ReSharper disable UnusedMember.Global

using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Shared.Snapshots
{
    public class SnapshotPlayerInfo
    {
        public int id;
        public Vector2 position;
        public PlayerInput input;

        public SnapshotPlayerInfo(int id, Vector2 position, PlayerInput input)
        {
            this.id = id;
            this.position = position;
            this.input = input;
        }
    }
}
