using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    [RequireComponent(typeof(PlayerAnimator))]
    [RequireComponent(typeof(Colorable))]
    public class ClientPlayer : MonoBehaviour
    {
        [Header("Parent component")]
        public BasePlayer basePlayer;

        [Header("Humble object components")]
        public PlayerAnimator animator;
        public ForciblyVisible forciblyVisible;

        [Header("Unity components")]
        public SpriteRenderer spriteRenderer;
        public Text nameLabel;

        public void Initialize(int playerId, string playerName, PlayerColor playerColor, bool isPlayerHost)
        {
            basePlayer.Initialize(playerId, playerName, playerColor, isPlayerHost);
        }
    }
}
