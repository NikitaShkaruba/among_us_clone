using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    [RequireComponent(typeof(Player))]
    public class ServerMovablePlayer : MonoBehaviour
    {
        private Player player;

        private void Start()
        {
            player = GetComponent<Player>();
        }

        public void FixedUpdate()
        {
            player.movable.MoveByPlayerControls(player.controllable.playerControls);
        }
    }
}
