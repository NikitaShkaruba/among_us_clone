using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    public class Player : MonoBehaviour
    {
        public PlayerInformation information;
        public Movable movable;
        public Controllable controllable;
        public RemoteControllable remoteControllable;
    }
}
