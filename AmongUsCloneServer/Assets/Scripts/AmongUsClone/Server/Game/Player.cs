using UnityEngine;

namespace AmongUsClone.Server.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public int id;
        public new string name;

        private PlayerInput playerInput;
        private new Rigidbody2D rigidbody;

        private const float MoveSpeed = 0.2f;

        public Vector2 Position => rigidbody.position;

        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        public void Initialize(int id, string name)
        {
            this.id = id;
            this.name = name;
            playerInput = new PlayerInput();
        }

        public void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector2 moveDirection = GetMoveDirection();

            Vector2 newPosition = rigidbody.position + moveDirection * MoveSpeed;
            rigidbody.MovePosition(newPosition);
        }

        // Todo: refactor into shared
        private Vector2 GetMoveDirection()
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

        public void UpdateInput(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }
    }
}
