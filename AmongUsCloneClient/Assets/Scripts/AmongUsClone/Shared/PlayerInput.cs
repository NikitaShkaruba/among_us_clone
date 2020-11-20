namespace AmongUsClone.Shared
{
    public class PlayerInput
    {
        public bool MoveTop;
        public bool MoveLeft;
        public bool MoveRight;
        public bool MoveBottom;

        public PlayerInput()
        {
        }

        private PlayerInput(bool moveTop, bool moveLeft, bool moveBottom, bool moveRight)
        {
            MoveTop = moveTop;
            MoveLeft = moveLeft;
            MoveBottom = moveBottom;
            MoveRight = moveRight;
        }

        public static PlayerInput Deserialize(bool[] serializedPlayerInput)
        {
            return new PlayerInput(
                serializedPlayerInput[0],
                serializedPlayerInput[1],
                serializedPlayerInput[2],
                serializedPlayerInput[3]
            );
        }

        public bool[] Serialize()
        {
            return new[]
            {
                MoveTop,
                MoveLeft,
                MoveBottom,
                MoveRight
            };
        }
    }
}
