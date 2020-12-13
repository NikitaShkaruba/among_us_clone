using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class Movable : MonoBehaviour
    {
        public float moveSpeed;

        public new Rigidbody2D rigidbody;

        public void MoveByPlayerInput(PlayerInput playerInput)
        {
            Vector2 relativePosition = GetMoveDirection(playerInput) * moveSpeed;
            MoveRelative(relativePosition);
        }

        public void MoveRelative(Vector2 relativePosition)
        {
            Move(rigidbody.position + relativePosition);
        }

        public void Move(Vector2 newPosition)
        {
            rigidbody.MovePosition(newPosition);
        }

        // Todo: refactor into shared
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

            return moveDirection;
        }
    }
}
