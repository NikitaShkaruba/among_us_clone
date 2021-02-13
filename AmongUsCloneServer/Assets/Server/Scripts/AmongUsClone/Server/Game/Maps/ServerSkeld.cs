using AmongUsClone.Server.Game.Maps.Surveillance;
using AmongUsClone.Shared.Game;
using UnityEngine;

namespace AmongUsClone.Server.Game.Maps
{
    public class ServerSkeld : MonoBehaviour
    {
        [Header("Parent component")]
        public Skeld sharedSkeld;

        [Header("Interactable")]
        public AdminPanel adminPanel;
        public SecurityPanel securityPanel;
    }
}
