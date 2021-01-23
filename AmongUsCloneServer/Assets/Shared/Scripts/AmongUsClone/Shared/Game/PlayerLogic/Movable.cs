// ReSharper disable UnusedMember.Global

using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movable : MonoBehaviour
    {
        public Rigidbody2D rigidbody2D;
        public float moveSpeed;

        public bool isDisabled;

        public void MoveByPlayerInput(PlayerInput playerInput)
        {
            Vector2 relativePosition = GetMoveDirection(playerInput) * moveSpeed * Time.fixedDeltaTime;
            MoveRelative(relativePosition);
        }

        private void MoveRelative(Vector2 relativePosition)
        {
            Vector2 newPosition = (Vector2) transform.position + relativePosition;
            Move(newPosition);
        }

        public void Move(Vector2 newPosition)
        {
            if (isDisabled)
            {
                return;
            }

            rigidbody2D.MovePosition(newPosition);
        }

        public void Teleport(Vector2 newPosition)
        {
            transform.position = newPosition;
        }

        private static Vector2 GetMoveDirection(PlayerInput playerInput)
        {
            Vector2 moveDirection = new Vector2(0f, 0f);

            if (playerInput.moveTop)
            {
                moveDirection.y++;
            }

            if (playerInput.moveLeft)
            {
                moveDirection.x--;
            }

            if (playerInput.moveBottom)
            {
                moveDirection.y--;
            }

            if (playerInput.moveRight)
            {
                moveDirection.x++;
            }

            // Prevent fast diagonal movement
            moveDirection = Vector2.ClampMagnitude(moveDirection, 1f);

            return moveDirection;
        }
    }
}
