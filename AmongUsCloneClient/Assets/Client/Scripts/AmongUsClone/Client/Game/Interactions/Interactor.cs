using System;
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
        [SerializeField] private InputReader inputReader;

        private bool interactButtonPressed;

        private Interactable chosenInteractableLastFrame;

        public Action<Interactable> newInteractableChosen;

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

            BroadcastToInteractablesTheirStates(chosen);
            CallInteractableChangedIfNeeded(chosen);
        }

        private void Interact()
        {
            if (chosen == null)
            {
                Logger.LogError(LoggerSection.Interactions, "Unable to interact without any chosen interactable");
                return;
            }

            chosen.Interact();
        }

        private void CallInteractableChangedIfNeeded(Interactable interactable)
        {
            bool wasNullAndNowNot = chosenInteractableLastFrame == null && interactable != null;
            bool wasNotNullButNowIs = chosenInteractableLastFrame != null && interactable == null;
            bool notNullAndChangedType = chosenInteractableLastFrame != null && interactable != null && interactable.type != chosenInteractableLastFrame.type;

            if (wasNullAndNowNot || wasNotNullButNowIs || notNullAndChangedType)
            {
                newInteractableChosen?.Invoke(interactable);
            }
        }

        private void BroadcastToInteractablesTheirStates(Interactable possibleInteractable)
        {
            foreach (Interactable interactable in chosables)
            {
                if (interactable == null)
                {
                    continue;
                }

                if (interactable == possibleInteractable)
                {
                    interactable.NoteThatMayBeSelected();
                }
                else
                {
                    interactable.NoteThatMayNotBeSelected();
                }
            }
        }
    }
}
