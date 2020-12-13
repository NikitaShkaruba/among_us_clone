using System.Collections.Generic;
using AmongUsClone.Server.Game.PlayerLogic;

namespace AmongUsClone.Server.Game.Snapshots
{
    /**
     * Snapshot with an information about every object for each player
     */
    public readonly struct GameSnapshot
    {
        public readonly int id;
        public readonly List<Player> players;

        public GameSnapshot(int id, List<Player> players)
        {
            this.id = id;
            this.players = players;
        }
    }
}
