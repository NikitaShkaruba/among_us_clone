using AmongUsClone.Client.Game.Lobby.UI;
using AmongUsClone.Client.UI.InteractButtons;
using UnityEngine;

namespace AmongUsClone.Client.Game.Lobby
{
    // Todo: decouple components from lobby prefab into lobby scene
    public class Lobby : MonoBehaviour
    {
        public GameObject playersContainer;
        public GameStartable gameStartable;

        public LobbyInteractButton interactButton;
    }
}
