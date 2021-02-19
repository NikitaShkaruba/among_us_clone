using System;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Logging;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Interactions
{
    /**
     * Object that interacts with interactables
     */
    public class Interactor : NearbyMonoBehavioursChooser<Interactable>
    {
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private ClientControllablePlayer clientControllablePlayer;

        private bool interactButtonPressed;

        private Interactable chosenInteractableLastFrame;

        public Action newInteractableChosen;

        private void OnEnable()
        {
            scenesManager.onSceneUpdate += CacheChosables;
        }

        private void OnDisable()
        {
            scenesManager.onSceneUpdate -= CacheChosables;
        }

        private new void FixedUpdate()
        {
            chosenInteractableLastFrame = chosen;
            base.FixedUpdate();

            BroadcastToInteractablesTheirStates();
            CallInteractableChangedIfNeeded();

            if (clientControllablePlayer.clientControllable.Interact)
            {
                Interact();
            }
        }

        public void Interact()
        {
            if (chosen == null)
            {
                Logger.LogNotice(LoggerSection.Interactions, "Unable to interact without any chosen interactable");
                return;
            }

            if (IsInteractionDisabled())
            {
                Logger.LogNotice(LoggerSection.Interactions, "Not calling interaction, because interaction is disabled");
                return;
            }

            chosen.Interact();
        }

        private bool IsInteractionDisabled()
        {
            if (playGamePhase.IsActive())
            {
                bool settingsMenuActive = playGamePhase.clientSkeld.playGamePhaseUserInterface.activeSceneUserInterface.settingsButton.SettingsMenuActive;

                return settingsMenuActive;
            }

            if (lobbyGamePhase.IsActive())
            {
                return lobbyGamePhase.lobby.activeSceneUserInterface.settingsButton.SettingsMenuActive;
            }

            return false;
        }

        private void CallInteractableChangedIfNeeded()
        {
            bool wasNullAndNowNot = chosenInteractableLastFrame == null && chosen != null;
            bool wasNotNullButNowIs = chosenInteractableLastFrame != null && chosen == null;
            bool notNullAndChangedType = chosenInteractableLastFrame != null && chosen != null && chosen.type != chosenInteractableLastFrame.type;

            if (wasNullAndNowNot || wasNotNullButNowIs || notNullAndChangedType)
            {
                newInteractableChosen?.Invoke();
            }
        }

        private void BroadcastToInteractablesTheirStates()
        {
            foreach (Interactable interactable in chosables)
            {
                if (interactable == null)
                {
                    continue;
                }

                if (interactable == chosen)
                {
                    interactable.ChooseAsClosestInteractable();
                }
                else
                {
                    interactable.DiscardAsClosestInteractable();
                }
            }
        }
    }
}
