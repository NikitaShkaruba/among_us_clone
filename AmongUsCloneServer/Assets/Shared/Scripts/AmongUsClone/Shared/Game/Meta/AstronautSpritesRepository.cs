using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Shared.Game.Meta
{
    public class AstronautSpritesRepository : MonoBehaviour
    {
        public Sprite redSprite;
        public Sprite blueSprite;
        public Sprite greenSprite;
        public Sprite yellowSprite;
        public Sprite pinkSprite;
        public Sprite orangeSprite;
        public Sprite purpleSprite;
        public Sprite blackSprite;
        public Sprite brownSprite;
        public Sprite cyanSprite;
        public Sprite limeSprite;
        public Sprite whiteSprite;

        public static AstronautSpritesRepository instance;

        private void Awake()
        {
            if (instance != null)
            {
                Logger.LogCritical(SharedLoggerSection.PlayerColors, "Attempt to instantiate singleton second time");
            }

            instance = this;
        }
    }
}
