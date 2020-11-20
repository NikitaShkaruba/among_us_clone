using AmongUsClone.Server.Networking;
using AmongUsClone.Shared;
using AmongUsClone.Shared.DataStructures;

namespace AmongUsClone.Server
{
    public class Player
    {
        public readonly int id;
        public readonly string name;

        private PlayerInput playerInput;
        public readonly Vector2 position;

        private const float MoveSpeed = 5f / Server.TicksPerSecond; // This matches client's fixedUpdateTime
        
        public Player(int id, string name, Vector2 position)
        {
            this.id = id;
            this.name = name;
            this.position = position;
            playerInput = new PlayerInput();
        }

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector2 moveDirection = GetMoveDirection();

            position.x += moveDirection.x * MoveSpeed;
            position.y += moveDirection.y * MoveSpeed;

            PacketsSender.SendPositionPacket(this);
        }

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
