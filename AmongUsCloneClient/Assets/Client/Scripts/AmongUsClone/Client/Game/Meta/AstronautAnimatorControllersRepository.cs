using UnityEngine;

namespace AmongUsClone.Client.Game.Meta
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    [CreateAssetMenu(fileName = "AstronautAnimatorControllersRepository", menuName = "AstronautAnimatorControllersRepository")]
    public class AstronautAnimatorControllersRepository : ScriptableObject
    {
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
    }
}
