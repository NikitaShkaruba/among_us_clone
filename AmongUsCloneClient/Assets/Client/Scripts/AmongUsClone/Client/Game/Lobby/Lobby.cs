using AmongUsClone.Client.Game.Lobby.UI;
using AmongUsClone.Client.UI.InteractButtons;
using AmongUsClone.Shared.Game;
using UnityEngine;

namespace AmongUsClone.Client.Game.Lobby
{
    [RequireComponent(typeof(PlayersContainable))]
    [RequireComponent(typeof(GameStartable))]
    public class Lobby : MonoBehaviour
    {
        public PlayersContainable playersContainable;
        public GameStartable gameStartable;

        public PlayersCounter playersCounter;
        public LobbyInteractButton interactButton;
    }
}
