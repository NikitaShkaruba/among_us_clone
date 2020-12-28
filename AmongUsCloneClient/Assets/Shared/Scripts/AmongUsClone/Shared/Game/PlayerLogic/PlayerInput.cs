// ReSharper disable FieldCanBeMadeReadOnly.Global
// ReSharper disable UnusedMember.Global

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class PlayerInput
    {
        public int id;

        public bool moveTop;
        public bool moveLeft;
        public bool moveRight;
        public bool moveBottom;

        public PlayerInput()
        {
        }

        private PlayerInput(int id, bool moveTop, bool moveLeft, bool moveBottom, bool moveRight)
        {
            this.id = id;

            this.moveTop = moveTop;
            this.moveLeft = moveLeft;
            this.moveBottom = moveBottom;
            this.moveRight = moveRight;
        }

        public static PlayerInput Deserialize(int id, bool[] serializedPlayerInput)
        {
            return new PlayerInput(
                id,
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
                moveTop,
                moveLeft,
                moveBottom,
                moveRight
            };
        }

        public PlayerInput Clone()
        {
            return new PlayerInput
            {
                id = id,
                moveTop = moveTop,
                moveBottom = moveBottom,
                moveLeft = moveLeft,
                moveRight = moveRight
            };
        }

        public override string ToString()
        {
            return "(" +
                   $"w: {SerializeInputValue(moveTop)}, " +
                   $"a: {SerializeInputValue(moveLeft)}, " +
                   $"s: {SerializeInputValue(moveBottom)}, " +
                   $"d: {SerializeInputValue(moveRight)}" +
                   ")";
        }

        private static int SerializeInputValue(bool inputValue)
        {
            return inputValue ? 1 : 0;
        }
    }
}
