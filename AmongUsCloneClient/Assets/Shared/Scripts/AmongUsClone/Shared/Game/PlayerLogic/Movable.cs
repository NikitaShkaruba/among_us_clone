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

        public void MoveByPlayerControls(PlayerControls playerControls)
        {
            Vector2 relativePosition = GetMoveDirection(playerControls) * moveSpeed * Time.fixedDeltaTime;
            MoveRelative(relativePosition);
        }

        public void MoveRelative(Vector2 relativePosition)
        {
            Move(rigidbody.position + relativePosition);
        }

        public void Move(Vector2 newPosition)
        {
            // Todo: replace with MovePosition
            // rigidbody.MovePosition(newPosition);
            transform.position = newPosition;
        }

        private static Vector2 GetMoveDirection(PlayerControls playerControls)
        {
            Vector2 moveDirection = new Vector2(0f, 0f);

            if (playerControls.moveTop)
            {
                moveDirection.y++;
            }

            if (playerControls.moveLeft)
            {
                moveDirection.x--;
            }

            if (playerControls.moveBottom)
            {
                moveDirection.y--;
            }

            if (playerControls.moveRight)
            {
                moveDirection.x++;
            }

            return moveDirection;
        }
    }
}
