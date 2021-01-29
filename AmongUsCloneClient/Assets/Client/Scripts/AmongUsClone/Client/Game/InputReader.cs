using System;
using AmongUsClone.Client.Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AmongUsClone.Client.Game
{
    // [CreateAssetMenu(fileName = "InputReader", menuName = "InputReader", order = 0)]
    public class InputReader : ScriptableObject, GameInputActionCollection.ICrewmateControlsActions
    {
        private GameInputActionCollection gameInputActionCollection;

        public Action<Vector2> onMove = delegate { };
        public Action onInteract = delegate { };
        public Action onToggleMinimap = delegate { };

        private void OnEnable()
        {
            if (gameInputActionCollection == null) ;
            {
                gameInputActionCollection = new GameInputActionCollection();
                gameInputActionCollection.CrewmateControls.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        public void EnableGameplayInput()
        {
            gameInputActionCollection.CrewmateControls.Enable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            onMove.Invoke(context.ReadValue<Vector2>());
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }

            onInteract.Invoke();
        }

        public void OnToggleMiniMap(InputAction.CallbackContext context)
        {
            if (context.phase != InputActionPhase.Performed)
            {
                return;
            }

            onToggleMinimap.Invoke();
        }
    }
}
