using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Meta
{
    public class AstronautAnimatorControllersRepository : MonoBehaviour
    {
        public static AstronautAnimatorControllersRepository instance;

        public RuntimeAnimatorController redAnimatorController;
        public RuntimeAnimatorController blueAnimatorController;
        public RuntimeAnimatorController greenAnimatorController;
        public RuntimeAnimatorController yellowAnimatorController;
        public RuntimeAnimatorController pinkAnimatorController;
        public RuntimeAnimatorController orangeAnimatorController;
        public RuntimeAnimatorController purpleAnimatorController;
        public RuntimeAnimatorController blackAnimatorController;
        public RuntimeAnimatorController brownAnimatorController;
        public RuntimeAnimatorController cyanAnimatorController;
        public RuntimeAnimatorController limeAnimatorController;
        public RuntimeAnimatorController whiteAnimatorController;

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
