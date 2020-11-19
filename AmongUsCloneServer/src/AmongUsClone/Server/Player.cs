using AmongUsClone.Server.Networking;
using AmongUsClone.Shared.DataStructures;

namespace AmongUsClone.Server
{
    public class Player
    {
        public readonly int Id;
        public readonly string Name;

        private bool[] inputs;
        public readonly Vector2 Position;

        private const float MoveSpeed = 5f / Server.TicksPerSecond; // This matches client's fixedUpdateTime
        
        public Player(int id, string name, Vector2 position)
        {
            Id = id;
            Name = name;
            Position = position;
            inputs = new bool[4];
        }

        public void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector2 moveDirection = GetMoveDirection();

            Position.X += moveDirection.X * MoveSpeed;
            Position.Y += moveDirection.Y * MoveSpeed;

            PacketsSender.SendPositionPacket(this);
        }

        private Vector2 GetMoveDirection()
        {
            Vector2 moveDirection = new Vector2(0f, 0f);

            if (inputs[0])
            {
                moveDirection.Y++;
            }

            if (inputs[1])
            {
                moveDirection.X--;
            }

            if (inputs[2])
            {
                moveDirection.Y--;
            }

            if (inputs[3])
            {
                moveDirection.X++;
            }

            return moveDirection;
        }

        public void SetInputs(bool[] inputs)
        {
            this.inputs = inputs;
        }
    }
}
