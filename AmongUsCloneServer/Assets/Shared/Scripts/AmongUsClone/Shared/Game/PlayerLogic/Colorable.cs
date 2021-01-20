using System;
using AmongUsClone.Shared.Game.Meta;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Colorable : MonoBehaviour
    {
        public AstronautSprites astronautSprites;

        public SpriteRenderer spriteRenderer;
        public PlayerColor color;

        public Action colorChanged;

        public void Start()
        {
            spriteRenderer.sprite = GetSprite();
        }

        public void Initialize(PlayerColor color)
        {
            this.color = color;
        }

        private Sprite GetSprite()
        {
            switch (color)
            {
                case PlayerColor.Red:
                    return astronautSprites.redSprite;

                case PlayerColor.Blue:
                    return astronautSprites.blueSprite;

                case PlayerColor.Green:
                    return astronautSprites.greenSprite;

                case PlayerColor.Yellow:
                    return astronautSprites.yellowSprite;

                case PlayerColor.Pink:
                    return astronautSprites.pinkSprite;

                case PlayerColor.Orange:
                    return astronautSprites.orangeSprite;

                case PlayerColor.Purple:
                    return astronautSprites.purpleSprite;

                case PlayerColor.Black:
                    return astronautSprites.blackSprite;

                case PlayerColor.Brown:
                    return astronautSprites.brownSprite;

                case PlayerColor.Cyan:
                    return astronautSprites.cyanSprite;

                case PlayerColor.Lime:
                    return astronautSprites.limeSprite;

                case PlayerColor.White:
                    return astronautSprites.whiteSprite;

                default:
                    Logger.LogError(SharedLoggerSection.PlayerColors, $"Undefined sprite for color {Helpers.GetEnumName(color)}");
                    return astronautSprites.redSprite;
            }
        }

        public void ChangeColor(PlayerColor color)
        {
            this.color = color;
            spriteRenderer.sprite = GetSprite();

            colorChanged?.Invoke();
        }
    }
}
