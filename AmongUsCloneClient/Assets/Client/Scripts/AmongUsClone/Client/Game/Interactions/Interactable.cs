using UnityEngine;

namespace AmongUsClone.Client.Game.Interactions
{
    // Todo: fix wrong shader highlighting when standing over the interactable (even from a far distance)
    /**
     * Object which can be interacted with
     */
    public abstract class Interactable : MonoBehaviour
    {
        [Header("Interactable base")]
        public InteractableType type;

        [Header("Visual highlighting")]
        [SerializeField] private new Renderer renderer;
        [SerializeField] private InteractableHighlightMaterials interactableHighlightMaterials;

        private bool wasInteractableFrameBefore;

        /**
         * Callback that is getting called when player presses interaction key near interactable
         */
        public abstract void Interact();

        public void ChooseAsClosestInteractable()
        {
            if (wasInteractableFrameBefore)
            {
                return;
            }

            renderer.material = interactableHighlightMaterials.materialWithOutlineAndHighlight;
            wasInteractableFrameBefore = true;
        }

        public void DiscardAsClosestInteractable()
        {
            if (!wasInteractableFrameBefore)
            {
                return;
            }

            renderer.material = interactableHighlightMaterials.materialWithOutline;
            wasInteractableFrameBefore = false;
        }
    }
}
