using UnityEngine;

namespace AmongUsClone.Server.Game.Interactions
{
    /**
     * Object which can be interacted with
     */
    public abstract class Interactable : MonoBehaviour
    {
        /**
         * Callback that is getting called when player presses interaction key near interactable
         */
        public abstract void Interact(int playerId);
    }
}
