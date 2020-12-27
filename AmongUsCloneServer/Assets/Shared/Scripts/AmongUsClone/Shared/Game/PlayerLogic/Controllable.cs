// ReSharper disable UnusedMember.Global

using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class Controllable : MonoBehaviour
    {
        public readonly PlayerInput playerInput = new PlayerInput();

        public void UpdateInput(PlayerInput playerInput)
        {
            this.playerInput.moveTop = playerInput.moveTop;
            this.playerInput.moveLeft = playerInput.moveLeft;
            this.playerInput.moveRight = playerInput.moveRight;
            this.playerInput.moveBottom = playerInput.moveBottom;
        }
    }
}
