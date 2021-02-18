using System;
using System.Collections.Generic;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Shared.Game.Interactions;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Maps.Surveillance
{
    [RequireComponent(typeof(PlayersLockable))]
    public class AdminPanel : Interactable
    {
        [Header("Scriptable objects")]
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PlayGamePhase playGamePhase;

        [Header("Self fields")]
        [SerializeField] private GameObject adminPanelMinimap;
        [SerializeField] private Button closeButton;
        [SerializeField] private AdminPanelCrewMateIconsShowable adminPanelCrewMateIconsShowable;
        [SerializeField] private PlayersLockable playersLockable;
        public bool isControlledPlayerViewing;

        public Action onInteraction;

        public void OnEnable()
        {
            closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        public void OnDisable()
        {
            closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        public override void Interact()
        {
            if (playGamePhase.clientSkeld.playGamePhaseUserInterface.minimapButton.IsMinimapShown)
            {
                return;
            }

            isControlledPlayerViewing = !isControlledPlayerViewing;

            if (isControlledPlayerViewing)
            {
                adminPanelMinimap.SetActive(true);
                playersLockable.Add(playersManager.controlledClientPlayer.basePlayer.information.id);
                Logger.LogEvent(SharedLoggerSection.Interactions, "Started viewing admin panel");
            }
            else
            {
                adminPanelMinimap.SetActive(false);
                playersLockable.Remove(playersManager.controlledClientPlayer.basePlayer.information.id);
                Logger.LogEvent(SharedLoggerSection.Interactions, "Stopped viewing admin panel");
            }

            onInteraction?.Invoke();
        }

        public void UpdateMinimap(Dictionary<int, int> gameSnapshotAdminPanelPositions)
        {
            adminPanelCrewMateIconsShowable.UpdateIcons(gameSnapshotAdminPanelPositions);
        }

        private void OnCloseButtonClick()
        {
            playersManager.controlledClientPlayer.clientControllable.OnInteract();
        }
    }
}
