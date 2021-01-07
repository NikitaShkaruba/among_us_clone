using AmongUsClone.Client.Game.Lobby.UI;
using AmongUsClone.Shared.Game;
using UnityEngine;

namespace AmongUsClone.Client.Game.Lobby
{
    [RequireComponent(typeof(PlayersContainable))]
    public class Lobby : MonoBehaviour
    {
        public PlayersContainable playersContainable;
        public PlayersCounter playersCounter;
    }
}
