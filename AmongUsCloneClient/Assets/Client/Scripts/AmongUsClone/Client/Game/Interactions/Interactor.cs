using System;
using AmongUsClone.Client.Game.GamePhaseManagers;
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
        [SerializeField] private InputReader inputReader;

        private bool interactButtonPressed;

        private Interactable chosenInteractableLastFrame;

        public Action newInteractableChosen;

        private void OnEnable()
        {
            scenesManager.onSceneUpdate += CacheChosables;
            inputReader.onInteract += Interact;
        }

        private void OnDisable()
        {
            scenesManager.onSceneUpdate -= CacheChosables;
            inputReader.onInteract -= Interact;
        }

        private new void Update()
        {
            chosenInteractableLastFrame = chosen;
            base.Update();

            BroadcastToInteractablesTheirStates();
            CallInteractableChangedIfNeeded();
        }

        private void Interact()
        {
            if (chosen == null)
            {
                Logger.LogError(LoggerSection.Interactions, "Unable to interact without any chosen interactable");
                return;
            }

            if (!IsAbleToInteract())
            {
                Logger.LogNotice(LoggerSection.Interactions, "Not calling interaction, because settings menu is active");
                return;
            }

            chosen.Interact();
        }

        private bool IsAbleToInteract()
        {
            if (playGamePhase.clientSkeld != null)
            {
                bool settingsMenuActive = playGamePhase.clientSkeld.playGamePhaseUserInterface.activeSceneUserInterface.settingsButton.SettingsMenuActive;

                return !settingsMenuActive;
            }

            if (lobbyGamePhase.lobby != null)
            {
                return !lobbyGamePhase.lobby.activeSceneUserInterface.settingsButton.SettingsMenuActive;
            }

            return true;
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
