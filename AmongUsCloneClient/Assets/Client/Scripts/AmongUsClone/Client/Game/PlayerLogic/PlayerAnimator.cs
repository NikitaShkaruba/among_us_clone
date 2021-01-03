using UnityEngine;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    [RequireComponent(typeof(Player))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int animatorPropertyIsMoving = Animator.StringToHash("IsMoving");
        private static readonly int animatorPropertyIsLookingRight = Animator.StringToHash("IsLookingRight");

        [SerializeField] private Player player;
        [SerializeField] private Animator animator;

        private bool isLookingRight;

        public void Update()
        {
            UpdateIsLookingRight();

            animator.SetBool(animatorPropertyIsMoving, player.controllable.IsMoving());
            animator.SetBool(animatorPropertyIsLookingRight, isLookingRight);
        }

        private void UpdateIsLookingRight()
        {
            if (player.controllable.playerInput.moveRight)
            {
                isLookingRight = true;
            }
            else if (player.controllable.playerInput.moveLeft)
            {
                isLookingRight = false;
            }
        }
    }
}
