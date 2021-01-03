// ReSharper disable UnusedMember.Global

using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class Controllable : MonoBehaviour
    {
        public PlayerInput playerInput = new PlayerInput();

        public bool IsMoving()
        {
            return playerInput.moveBottom ||
                   playerInput.moveLeft ||
                   playerInput.moveTop ||
                   playerInput.moveRight;
        }
    }
}
