using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    public class ClientControllablePlayer : MonoBehaviour
    {
        [Header("Parent components")]
        public ClientPlayer clientPlayer;
        public BasePlayer basePlayer;

        [Header("Parent components")]
        public ClientControllable clientControllable;
        public Interactor interactor;
        public Viewable viewable;
        public MinimapIconOwnable minimapIconOwnable;
    }
}
