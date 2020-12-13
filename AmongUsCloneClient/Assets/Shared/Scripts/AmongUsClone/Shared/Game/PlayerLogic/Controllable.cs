using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class Controllable : MonoBehaviour
    {
        public readonly PlayerControls playerControls = new PlayerControls();

        public void UpdateControls(PlayerControls playerControls)
        {
            this.playerControls.moveTop = playerControls.moveTop;
            this.playerControls.moveLeft = playerControls.moveLeft;
            this.playerControls.moveRight = playerControls.moveRight;
            this.playerControls.moveBottom = playerControls.moveBottom;
        }
    }
}
