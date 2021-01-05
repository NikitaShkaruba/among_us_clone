using AmongUsClone.Shared.Game.Meta;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Colorable : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public PlayerColor color;

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
                    return AstronautSpritesRepository.instance.redSprite;

                case PlayerColor.Blue:
                    return AstronautSpritesRepository.instance.blueSprite;

                case PlayerColor.Green:
                    return AstronautSpritesRepository.instance.greenSprite;

                case PlayerColor.Yellow:
                    return AstronautSpritesRepository.instance.yellowSprite;

                case PlayerColor.Pink:
                    return AstronautSpritesRepository.instance.pinkSprite;

                case PlayerColor.Orange:
                    return AstronautSpritesRepository.instance.orangeSprite;

                case PlayerColor.Purple:
                    return AstronautSpritesRepository.instance.purpleSprite;

                case PlayerColor.Black:
                    return AstronautSpritesRepository.instance.blackSprite;

                case PlayerColor.Brown:
                    return AstronautSpritesRepository.instance.brownSprite;

                case PlayerColor.Cyan:
                    return AstronautSpritesRepository.instance.cyanSprite;

                case PlayerColor.Lime:
                    return AstronautSpritesRepository.instance.limeSprite;

                case PlayerColor.White:
                    return AstronautSpritesRepository.instance.whiteSprite;

                default:
                    Logger.LogError(SharedLoggerSection.PlayerColors, $"Undefined sprite for color {Helpers.GetEnumName(color)}");
                    return AstronautSpritesRepository.instance.redSprite;
            }
        }
    }
}
