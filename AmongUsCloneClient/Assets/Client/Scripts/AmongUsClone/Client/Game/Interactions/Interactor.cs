using System;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Shared.Scenes;
using JetBrains.Annotations;
using UnityEngine;

namespace AmongUsClone.Client.Game.Interactions
{
    /**
     * Object that interacts with interactables
     */
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private ScenesManager scenesManager;

        public float interactionDistance;
        public KeyCode interactionKey = KeyCode.E;

        private List<Interactable> interactables;
        [CanBeNull] public Interactable chosenInteractable;
        private Interactable chosenInteractableLastFrame;

        public Action<Interactable> newInteractableChosen;

        private void Start()
        {
            scenesManager.onScenesUpdate += CacheInteractables;
            CacheInteractables();
        }

        private void Update()
        {
            chosenInteractableLastFrame = chosenInteractable;
            chosenInteractable = FindClosestInteractableInRange();

            BroadcastToInteractablesTheirStates(chosenInteractable);
            CallInteractableChangedIfNeeded(chosenInteractable);

            if (chosenInteractable != null && Input.GetKeyDown(interactionKey))
            {
                chosenInteractable.Interact();
            }
        }

        public void CacheInteractables()
        {
            interactables = FindObjectsOfType<Interactable>().ToList();
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

        [CanBeNull]
        private Interactable FindClosestInteractableInRange()
        {
            float distanceToClosesInteractable = float.PositiveInfinity;
            Interactable closestInteractable = null;

            foreach (Interactable interactable in interactables)
            {
                Vector3 positionDifferenceWithControlledPlayer = transform.position - interactable.transform.position;
                if (positionDifferenceWithControlledPlayer.magnitude > interactionDistance || positionDifferenceWithControlledPlayer.magnitude > distanceToClosesInteractable)
                {
                    continue;
                }

                distanceToClosesInteractable = positionDifferenceWithControlledPlayer.magnitude;
                closestInteractable = interactable;
            }

            return closestInteractable;
        }

        private void BroadcastToInteractablesTheirStates(Interactable possibleInteractable)
        {
            foreach (Interactable interactable in interactables)
            {
                if (interactable == null)
                {
                    continue;
                }

                if (interactable == possibleInteractable)
                {
                    interactable.NoteThatInteractionMayBeSelected();
                }
                else
                {
                    interactable.NotThatInteractionMayNotBeSelected();
                }
            }
        }
    }
}
