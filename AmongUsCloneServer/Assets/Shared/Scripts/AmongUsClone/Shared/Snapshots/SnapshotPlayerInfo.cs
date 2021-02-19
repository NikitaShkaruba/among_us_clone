// ReSharper disable UnusedMember.Global

using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Shared.Snapshots
{
    public class SnapshotPlayerInfo
    {
        public int id;
        public string name;
        public Vector2 position;
        public PlayerInput input;
        public PlayerColor color;
        public bool isImpostor;
        public bool unseen;
        public bool isLobbyHost;

        public SnapshotPlayerInfo(int id, string name, bool isLobbyHost, PlayerInput input, Vector2 position, bool unseen, PlayerColor color, bool isImpostor)
        {
            this.id = id;
            this.name = name;
            this.isLobbyHost = isLobbyHost;
            this.input = input;
            this.position = position;
            this.unseen = unseen;
            this.color = color;
            this.isImpostor = isImpostor;
        }

        public SnapshotPlayerInfo(SnapshotPlayerInfo prototypeSnapshotPlayerInfo) : this(prototypeSnapshotPlayerInfo.id, prototypeSnapshotPlayerInfo.name, prototypeSnapshotPlayerInfo.isLobbyHost, prototypeSnapshotPlayerInfo.input, prototypeSnapshotPlayerInfo.position, prototypeSnapshotPlayerInfo.unseen, prototypeSnapshotPlayerInfo.color, prototypeSnapshotPlayerInfo.isImpostor)
        {
        }
    }
}
