using UnityEngine;

namespace AmongUsClone.Client.Game.Interactions
{
    /**
     * Object which can be interacted with
     */
    public abstract class Interactable : MonoBehaviour
    {
        public InteractableType type;
        private bool wasInteractableFrameBefore;

        public void NoteThatMayBeSelected()
        {
            if (wasInteractableFrameBefore)
            {
                return;
            }

            SetHighlighting();
            wasInteractableFrameBefore = true;
        }

        public void NoteThatMayNotBeSelected()
        {
            if (!wasInteractableFrameBefore)
            {
                return;
            }

            RemoveHighlighting();
            wasInteractableFrameBefore = false;
        }

        /**
         * Callback that is getting called when the controlled player may interact with this object by pressing his interaction key
         */
        protected abstract void SetHighlighting();

        /**
         * Callback that is getting called when the controlled player loses an ability interact with this object by pressing his interaction key
         */
        protected abstract void RemoveHighlighting();

        /**
         * Callback that is getting called when player presses interaction key near interactable
         */
        public abstract void Interact();
    }
}
