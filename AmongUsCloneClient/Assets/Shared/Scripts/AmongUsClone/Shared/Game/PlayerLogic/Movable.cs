// ReSharper disable UnusedMember.Global

using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Movable : MonoBehaviour
    {
        public float moveSpeed;

        [HideInInspector] public new Rigidbody2D rigidbody;

        public void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public Vector2 MoveByPlayerInput(PlayerInput playerInput)
        {
            Vector2 relativePosition = GetMoveDirection(playerInput) * moveSpeed * Time.fixedDeltaTime;
            return MoveRelative(relativePosition);
        }

        public Vector2 MoveRelative(Vector2 relativePosition)
        {
            Vector2 newPosition = (Vector2)transform.position + relativePosition;
            Move(newPosition);

            return newPosition;
        }

        public void Move(Vector2 newPosition)
        {
            // Todo: replace with MovePosition
            // rigidbody.MovePosition(newPosition);
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

            return moveDirection;
        }
    }
}
