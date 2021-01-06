using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace AmongUsClone.Client.Game.Interactions
{
    /**
     * Object that interacts with interactables
     */
    public class Interactor : MonoBehaviour
    {
        public float interactionDistance;
        public KeyCode interactionKey = KeyCode.E;

        private List<Interactable> interactables;

        public void Start()
        {
            interactables = FindObjectsOfType<Interactable>().ToList();
        }

        public void Update()
        {
            Interactable interactable = FindClosestInteractableInRange();
            BroadcastToInteractablesTheirStates(interactable);

            if (interactable != null && Input.GetKeyDown(interactionKey))
            {
                interactable.Interact();
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
