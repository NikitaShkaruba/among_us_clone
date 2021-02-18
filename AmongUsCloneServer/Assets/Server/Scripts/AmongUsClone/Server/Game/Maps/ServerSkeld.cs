using AmongUsClone.Server.Game.Maps.Surveillance;
using AmongUsClone.Shared.Game;
using UnityEngine;

namespace AmongUsClone.Server.Game.Maps
{
    public class ServerSkeld : MonoBehaviour
    {
        [Header("Parent component")]
        public Skeld sharedSkeld; // Todo: replace all != null checks with a method call

        [Header("Interactable")]
        public AdminPanel adminPanel;
        public SecurityPanel securityPanel;
    }
}
