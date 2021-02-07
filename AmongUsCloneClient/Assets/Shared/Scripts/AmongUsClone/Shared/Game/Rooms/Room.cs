using UnityEngine;

namespace AmongUsClone.Shared.Game.Rooms
{
    public class Room : MonoBehaviour
    {
        [Header("Information")]
        public int id;
        public new string name;

        [Header("Humble object components")]
        public WalkingPlayersDetectable walkingPlayersDetectable;
    }
}
