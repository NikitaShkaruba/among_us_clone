using AmongUsClone.Client.Game.Maps.Surveillance;
using AmongUsClone.Client.UI;
using AmongUsClone.Client.UI.Buttons.ActionButtons;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Maps;
using UnityEngine;

namespace AmongUsClone.Client.Game.Maps
{
    public class ClientSkeld : MonoBehaviour
    {
        [Header("Parent component")]
        public Skeld sharedSkeld;

        [Header("Humble object components")]
        public PlayerSpawnable playerSpawnable;
        public InteractButton interactButton;
        public AdminPanel adminPanel;
        public SecurityPanel securityPanel;
        public PlayGamePhaseUserInterface playGamePhaseUserInterface;
    }
}
