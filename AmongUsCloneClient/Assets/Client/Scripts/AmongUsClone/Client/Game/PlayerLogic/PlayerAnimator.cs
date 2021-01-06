using AmongUsClone.Client.Game.Meta;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

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

        public void Start()
        {
            UpdateAnimatorController();
            player.colorable.colorChanged += UpdateAnimatorController;
        }

        public void OnDestroy()
        {
            player.colorable.colorChanged -= UpdateAnimatorController;
        }

        public void UpdateAnimatorController()
        {
            RuntimeAnimatorController animatorController;

            switch (player.colorable.color)
            {
                case PlayerColor.Red:
                    animatorController = AstronautAnimatorControllersRepository.instance.redAnimatorController;
                    break;

                case PlayerColor.Blue:
                    animatorController = AstronautAnimatorControllersRepository.instance.blueAnimatorController;
                    break;

                case PlayerColor.Green:
                    animatorController = AstronautAnimatorControllersRepository.instance.greenAnimatorController;
                    break;

                case PlayerColor.Yellow:
                    animatorController = AstronautAnimatorControllersRepository.instance.yellowAnimatorController;
                    break;

                case PlayerColor.Pink:
                    animatorController = AstronautAnimatorControllersRepository.instance.pinkAnimatorController;
                    break;

                case PlayerColor.Orange:
                    animatorController = AstronautAnimatorControllersRepository.instance.orangeAnimatorController;
                    break;

                case PlayerColor.Purple:
                    animatorController = AstronautAnimatorControllersRepository.instance.purpleAnimatorController;
                    break;

                case PlayerColor.Black:
                    animatorController = AstronautAnimatorControllersRepository.instance.blackAnimatorController;
                    break;

                case PlayerColor.Brown:
                    animatorController = AstronautAnimatorControllersRepository.instance.brownAnimatorController;
                    break;

                case PlayerColor.Cyan:
                    animatorController = AstronautAnimatorControllersRepository.instance.cyanAnimatorController;
                    break;

                case PlayerColor.Lime:
                    animatorController = AstronautAnimatorControllersRepository.instance.limeAnimatorController;
                    break;

                case PlayerColor.White:
                    animatorController = AstronautAnimatorControllersRepository.instance.whiteAnimatorController;
                    break;

                default:
                    Logger.LogError(SharedLoggerSection.PlayerColors, "Undefined player color");
                    return;
            }

            animator.runtimeAnimatorController = animatorController;
        }

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
