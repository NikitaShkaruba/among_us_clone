using System.Collections;
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
            StartCoroutine(DropPlayerControls());
        }

        // Todo: temporary solution. Think about something better
        private IEnumerator DropPlayerControls()
        {
            yield return new WaitForFixedUpdate();

            player.controllable.playerControls.moveTop = false;
            player.controllable.playerControls.moveLeft = false;
            player.controllable.playerControls.moveRight = false;
            player.controllable.playerControls.moveBottom = false;
        }

    }
}
