using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Helpers = AmongUsClone.Shared.Helpers;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.PlayerLogic
{
    public class MinimapIconOwnable : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer minimapIconSpriteRenderer;

        [SerializeField] private MinimapIconSprites minimapIconSprites;
        [SerializeField] private Player player;

        public void Initialize()
        {
            UpdateColor();
            player.colorable.colorChanged += UpdateColor;
        }

        private void UpdateColor()
        {
            minimapIconSpriteRenderer.sprite = GetSprite(player.colorable.color);
        }

        private Sprite GetSprite(PlayerColor playerColor)
        {
            switch (playerColor)
            {
                case PlayerColor.Red:
                    return minimapIconSprites.redSprite;

                case PlayerColor.Blue:
                    return minimapIconSprites.blueSprite;

                case PlayerColor.Green:
                    return minimapIconSprites.greenSprite;

                case PlayerColor.Yellow:
                    return minimapIconSprites.yellowSprite;

                case PlayerColor.Pink:
                    return minimapIconSprites.pinkSprite;

                case PlayerColor.Orange:
                    return minimapIconSprites.orangeSprite;

                case PlayerColor.Purple:
                    return minimapIconSprites.purpleSprite;

                case PlayerColor.Black:
                    return minimapIconSprites.blackSprite;

                case PlayerColor.Brown:
                    return minimapIconSprites.brownSprite;

                case PlayerColor.Cyan:
                    return minimapIconSprites.cyanSprite;

                case PlayerColor.Lime:
                    return minimapIconSprites.limeSprite;

                case PlayerColor.White:
                    return minimapIconSprites.whiteSprite;

                default:
                    Logger.LogError(SharedLoggerSection.PlayerColors, $"Undefined sprite for color {Helpers.GetEnumName(playerColor)}");
                    return minimapIconSprites.redSprite;
            }
        }
    }
}
