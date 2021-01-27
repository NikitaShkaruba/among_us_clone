using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    [RequireComponent(typeof(PlayerInformation))]
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Controllable))]
    [RequireComponent(typeof(RemoteControllable))]
    [RequireComponent(typeof(Colorable))]
    public class Player : MonoBehaviour
    {
        public PlayerInformation information;
        public Movable movable;
        public Controllable controllable;
        public RemoteControllable remoteControllable;
        public Colorable colorable;

        public SpriteRenderer spriteRenderer;

        public void Initialize(int playerId, string playerName, PlayerColor playerColor, bool isLookingRight, bool isLobbyHost)
        {
            information.Initialize(playerId, playerName, isLobbyHost);
            colorable.Initialize(playerColor);
            spriteRenderer.flipX = !isLookingRight;
        }
    }
}
