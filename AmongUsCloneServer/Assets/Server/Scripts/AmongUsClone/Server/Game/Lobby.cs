using AmongUsClone.Shared.Game;
using UnityEngine;

namespace AmongUsClone.Server.Game
{
    [RequireComponent(typeof(PlayersContainable))]
    public class Lobby : MonoBehaviour
    {
        public PlayersContainable playersContainable;
    }
}
