using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.PlayerLogic;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    [RequireComponent(typeof(PlayerInformation))]
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Controllable))]
    [RequireComponent(typeof(ClientControllable))]
    [RequireComponent(typeof(PlayerAnimator))]
    [RequireComponent(typeof(Colorable))]
    [RequireComponent(typeof(Interactor))]
    [RequireComponent(typeof(MinimapIconOwnable))]
    public class Player : MonoBehaviour
    {
        [Header("Every Player components")]
        public PlayerInformation information;
        public Movable movable;
        public Controllable controllable;
        public PlayerAnimator animator;
        public Colorable colorable;

        [Header("ClientControllable only components")]
        public ClientControllable clientControllable;
        public Interactor interactor;
        public MinimapIconOwnable minimapIconOwnable;

        [Header("Unity components")]
        public SpriteRenderer spriteRenderer;
        public Text nameLabel;

        public void Initialize(int playerId, string playerName, PlayerColor playerColor, bool isPlayerHost)
        {
            information.Initialize(playerId, playerName, isPlayerHost);
            colorable.Initialize(playerColor);

            if (minimapIconOwnable != null)
            {
                minimapIconOwnable.Initialize();
            }
        }
    }
}
