// ReSharper disable UnusedMember.Global

using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Shared.Snapshots
{
    public class SnapshotPlayerInfo
    {
        public int id;
        public Vector2 position;
        public PlayerInput input;
        public PlayerColor color;
        public bool isImpostor;

        public SnapshotPlayerInfo(int id, Vector2 position, PlayerInput input, PlayerColor color, bool isImpostor)
        {
            this.id = id;
            this.position = position;
            this.input = input;
            this.color = color;
            this.isImpostor = isImpostor;
        }
    }
}
