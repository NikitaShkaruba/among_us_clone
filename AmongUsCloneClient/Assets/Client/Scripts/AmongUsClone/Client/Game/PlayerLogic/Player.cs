using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.PlayerLogic;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    [RequireComponent(typeof(PlayerInformation))]
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Controllable))]
    [RequireComponent(typeof(ClientControllable))]
    [RequireComponent(typeof(PlayerAnimator))]
    [RequireComponent(typeof(Colorable))]
    [RequireComponent(typeof(Interactor))]
    public class Player : MonoBehaviour
    {
        public PlayerInformation information;
        public Movable movable;
        public Controllable controllable;
        public ClientControllable clientControllable;
        public PlayerAnimator animator;
        public Colorable colorable;
        public Interactor interactor;

        public void Initialize(int playerId, string playerName, PlayerColor playerColor)
        {
            information.Initialize(playerId, playerName);
            colorable.Initialize(playerColor);
        }
    }
}
