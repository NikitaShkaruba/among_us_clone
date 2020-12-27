using System.Collections.Generic;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    [RequireComponent(typeof(Player))]
    public class ServerPlayer : MonoBehaviour
    {
        private Player player;
        private readonly Queue<PlayerInput> queuedInputs = new Queue<PlayerInput>();

        private void Start()
        {
            player = GetComponent<Player>();
        }

        public void FixedUpdate()
        {
            if (queuedInputs.Count != 0)
            {
                player.controllable.UpdateInput(queuedInputs.Dequeue());
            }

            player.movable.MoveByPlayerInput(player.controllable.playerInput);
        }

        public void EnqueueInput(PlayerInput playerInput)
        {
            queuedInputs.Enqueue(playerInput);
        }
    }
}
