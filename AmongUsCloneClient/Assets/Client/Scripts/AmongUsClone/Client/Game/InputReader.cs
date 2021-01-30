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

        public Action<Vector2> onMove = delegate { }; // onMove is implemented with 4 different buttons and not with 2d Vector Composite, because I encounter a bug like there https://forum.unity.com/threads/new-input-system-move-2d-vector-null-in-build-unity-2019-4-15f1-lts-and-2019-4-17f1-lts.1032043/, and there is no solution for it
        public Action onInteract = delegate { };
        public Action onToggleMinimap = delegate { };

        private Vector2 moveDirection;

        private void OnEnable()
        {
            if (gameInputActionCollection == null)
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

        public void OnMoveUp(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                moveDirection.y += 1;
                onMove.Invoke(moveDirection);
            } else if (context.phase == InputActionPhase.Canceled)
            {
                moveDirection.y -= 1;
                onMove.Invoke(moveDirection);
            }
        }

        public void OnMoveRight(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                moveDirection.x += 1;
                onMove.Invoke(moveDirection);
            } else if (context.phase == InputActionPhase.Canceled)
            {
                moveDirection.x -= 1;
                onMove.Invoke(moveDirection);
            }
        }

        public void OnMoveDown(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                moveDirection.y -= 1;
                onMove.Invoke(moveDirection);
            } else if (context.phase == InputActionPhase.Canceled)
            {
                moveDirection.y += 1;
                onMove.Invoke(moveDirection);
            }
        }

        public void OnMoveLeft(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                moveDirection.x -= 1;
                onMove.Invoke(moveDirection);
            } else if (context.phase == InputActionPhase.Canceled)
            {
                moveDirection.x += 1;
                onMove.Invoke(moveDirection);
            }

            onMove.Invoke(moveDirection);
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
