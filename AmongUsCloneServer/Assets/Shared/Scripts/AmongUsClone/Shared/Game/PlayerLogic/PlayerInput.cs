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

        public bool interact;

        public bool startGame;

        public PlayerInput()
        {
        }

        public PlayerInput(int id, bool moveTop, bool moveLeft, bool moveBottom, bool moveRight, bool interact, bool startGame)
        {
            this.id = id;

            this.moveTop = moveTop;
            this.moveLeft = moveLeft;
            this.moveBottom = moveBottom;
            this.moveRight = moveRight;

            this.interact = interact;

            this.startGame = startGame;
        }

        public static PlayerInput Deserialize(int id, bool[] serializedInputValues)
        {
            return new PlayerInput(
                id,
                serializedInputValues[0],
                serializedInputValues[1],
                serializedInputValues[2],
                serializedInputValues[3],
                serializedInputValues[4],
                serializedInputValues[5]
            );
        }

        public bool[] SerializeValues()
        {
            return new[]
            {
                moveTop,
                moveLeft,
                moveBottom,
                moveRight,
                interact,
                startGame,
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
                moveRight = moveRight,
                interact = interact,
                startGame = startGame,
            };
        }

        public override string ToString()
        {
            return "(" +
                   $"w: {SerializeInputValue(moveTop)}, " +
                   $"a: {SerializeInputValue(moveLeft)}, " +
                   $"s: {SerializeInputValue(moveBottom)}, " +
                   $"d: {SerializeInputValue(moveRight)}" +
                   $"interact: {SerializeInputValue(interact)}" +
                   $"startGame: {SerializeInputValue(startGame)}" +
                   ")";
        }

        private static int SerializeInputValue(bool inputValue)
        {
            return inputValue ? 1 : 0;
        }
    }
}
