using AmongUsClone.Server.Game.Interactions;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    [RequireComponent(typeof(RemoteControllable))]
    [RequireComponent(typeof(Viewable))]
    [RequireComponent(typeof(NearbyInteractableChooser))]
    public class ServerPlayer : MonoBehaviour
    {
        [Header("Parent component")]
        public BasePlayer basePlayer;

        [Header("Humble object components")]
        public Viewable viewable;
        public RemoteControllable remoteControllable;
        public NearbyInteractableChooser nearbyInteractableChooser;

        [Header("Unity components")]
        public SpriteRenderer spriteRenderer;

        public void Initialize(int playerId, string playerName, PlayerColor playerColor, bool isLookingRight, bool isLobbyHost)
        {
            remoteControllable.isLobbyHost = isLobbyHost;
            basePlayer.Initialize(playerId, playerName, playerColor, isLobbyHost);
            spriteRenderer.flipX = !isLookingRight;
        }
    }
}
