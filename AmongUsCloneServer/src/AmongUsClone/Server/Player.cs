using AmongUsClone.Server.Networking;
using AmongUsClone.Shared;
using AmongUsClone.Shared.DataStructures;

namespace AmongUsClone.Server
{
    public class Player
    {
        public readonly int Id;
        public readonly string Name;

        private PlayerInput playerInput;
        public readonly Vector2 Position;

        private const float MoveSpeed = 5f / Server.TicksPerSecond; // This matches client's fixedUpdateTime
        
        public Player(int id, string name, Vector2 position)
        {
            Id = id;
            Name = name;
            Position = position;
            playerInput = new PlayerInput();
        }

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector2 moveDirection = GetMoveDirection();

            Position.x += moveDirection.x * MoveSpeed;
            Position.y += moveDirection.y * MoveSpeed;

            PacketsSender.SendPositionPacket(this);
        }

        private Vector2 GetMoveDirection()
        {
            Vector2 moveDirection = new Vector2(0f, 0f);

            if (playerInput.MoveTop)
            {
                moveDirection.y++;
            }

            if (playerInput.MoveLeft)
            {
                moveDirection.x--;
            }

            if (playerInput.MoveBottom)
            {
                moveDirection.y--;
            }

            if (playerInput.MoveRight)
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
