using System;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.GamePhaseManagers;
using UnityEngine;

namespace AmongUsClone.Client.UI.Buttons
{
    public class MinimapButton : MonoBehaviour
    {
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private InputReader inputReader;

        public GameObject minimap;
        public SettingsButton settingsButton;

        public Action onToggle;

        public bool IsMinimapShown => minimap.activeSelf;

        private void OnEnable()
        {
            inputReader.onToggleMinimap += Toggle;
        }

        private void OnDisable()
        {
            inputReader.onToggleMinimap -= Toggle;
        }

        public void Toggle()
        {
            if (settingsButton.SettingsMenuActive)
            {
                settingsButton.ToggleMenu();
                return;
            }

            if (playGamePhase.clientSkeld.adminPanel.isControlledPlayerViewing)
            {
                playersManager.controlledClientPlayer.clientControllable.OnInteract();
                return;
            }

            if (playGamePhase.clientSkeld.securityPanel.isControlledPlayerViewing)
            {
                playersManager.controlledClientPlayer.clientControllable.OnInteract();
                return;
            }

            minimap.SetActive(!minimap.activeSelf);

            onToggle?.Invoke();
        }
    }
}
