using AmongUsClone.Client.Game.Meta;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    [RequireComponent(typeof(ClientPlayer))]
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        private static readonly int animatorPropertyIsMoving = Animator.StringToHash("IsMoving");
        private static readonly int animatorPropertyIsLookingRight = Animator.StringToHash("IsLookingRight");

        [SerializeField] private AstronautAnimatorControllersRepository astronautAnimatorControllersRepository;
        [SerializeField] private ClientPlayer clientPlayer;
        [SerializeField] private Animator animator;

        public bool isLookingRight;

        public void Awake()
        {
            isLookingRight = !clientPlayer.spriteRenderer.flipX;
        }

        public void Start()
        {
            UpdateAnimatorController();
            clientPlayer.basePlayer.colorable.colorChanged += UpdateAnimatorController;
        }

        public void OnDestroy()
        {
            clientPlayer.basePlayer.colorable.colorChanged -= UpdateAnimatorController;
        }

        private void UpdateAnimatorController()
        {
            RuntimeAnimatorController animatorController;

            switch (clientPlayer.basePlayer.colorable.color)
            {
                case PlayerColor.Red:
                    animatorController = astronautAnimatorControllersRepository.redAnimatorController;
                    break;

                case PlayerColor.Blue:
                    animatorController = astronautAnimatorControllersRepository.blueAnimatorController;
                    break;

                case PlayerColor.Green:
                    animatorController = astronautAnimatorControllersRepository.greenAnimatorController;
                    break;

                case PlayerColor.Yellow:
                    animatorController = astronautAnimatorControllersRepository.yellowAnimatorController;
                    break;

                case PlayerColor.Pink:
                    animatorController = astronautAnimatorControllersRepository.pinkAnimatorController;
                    break;

                case PlayerColor.Orange:
                    animatorController = astronautAnimatorControllersRepository.orangeAnimatorController;
                    break;

                case PlayerColor.Purple:
                    animatorController = astronautAnimatorControllersRepository.purpleAnimatorController;
                    break;

                case PlayerColor.Black:
                    animatorController = astronautAnimatorControllersRepository.blackAnimatorController;
                    break;

                case PlayerColor.Brown:
                    animatorController = astronautAnimatorControllersRepository.brownAnimatorController;
                    break;

                case PlayerColor.Cyan:
                    animatorController = astronautAnimatorControllersRepository.cyanAnimatorController;
                    break;

                case PlayerColor.Lime:
                    animatorController = astronautAnimatorControllersRepository.limeAnimatorController;
                    break;

                case PlayerColor.White:
                    animatorController = astronautAnimatorControllersRepository.whiteAnimatorController;
                    break;

                default:
                    Logger.LogError(SharedLoggerSection.PlayerColors, "Undefined player color");
                    return;
            }

            animator.runtimeAnimatorController = animatorController;
        }

        private void Update()
        {
            UpdateIsLookingRight();

            animator.SetBool(animatorPropertyIsMoving, clientPlayer.basePlayer.controllable.IsMoving());
            animator.SetBool(animatorPropertyIsLookingRight, isLookingRight);
        }

        private void UpdateIsLookingRight()
        {
            if (clientPlayer.basePlayer.controllable.playerInput.moveRight)
            {
                isLookingRight = true;
            }
            else if (clientPlayer.basePlayer.controllable.playerInput.moveLeft)
            {
                isLookingRight = false;
            }
        }
    }
}
