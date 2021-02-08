using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Controllable))]
    [RequireComponent(typeof(Colorable))]
    public class BasePlayer : MonoBehaviour
    {
        public PlayerBaseInformation information;
        public Colorable colorable;
        public Movable movable;
        public Controllable controllable;
        public NameShowable nameShowable;
        public Impostorable impostorable;
        public GameHostable gameHostable;

        public void Initialize(int id, string name, PlayerColor color, bool isLobbyHost)
        {
            information.id = id;
            information.name = name;
            colorable.Initialize(color);
            gameHostable.isHost = isLobbyHost;
        }
    }
}
