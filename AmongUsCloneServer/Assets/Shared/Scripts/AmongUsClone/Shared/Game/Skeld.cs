using AmongUsClone.Shared.Game.Rooms;
using AmongUsClone.Shared.Maps;
using UnityEngine;

namespace AmongUsClone.Shared.Game
{
    public class Skeld : MonoBehaviour
    {
        public PlayerSpawnable playerSpawnable;
        public Room[] rooms;

        private void Start()
        {
            for (int roomId = 0; roomId < rooms.Length; roomId++)
            {
                rooms[roomId].id = roomId;
            }
        }
    }
}
