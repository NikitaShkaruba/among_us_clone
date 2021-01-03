using AmongUsClone.Client.PlayerLogic;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        public PlayerInformation information;
        public Movable movable;
        public Controllable controllable;
        public ClientControllable clientControllable;
        public PlayerAnimator playerAnimator;
    }
}
