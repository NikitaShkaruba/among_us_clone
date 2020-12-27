using System;
using System.Collections.Generic;
using AmongUsClone.Server.Snapshots;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    [RequireComponent(typeof(Player))]
    public class ServerPlayer : MonoBehaviour
    {
        private Player player;
        private readonly Queue<Tuple<int, PlayerInput>> queuedInputs = new Queue<Tuple<int, PlayerInput>>();

        private void Start()
        {
            player = GetComponent<Player>();
        }

        public void FixedUpdate()
        {
            if (queuedInputs.Count != 0)
            {
                (int inputId, PlayerInput playerInput) = queuedInputs.Dequeue();
                player.controllable.UpdateInput(playerInput);
                ProcessedPlayerInputs.Update(player.id, inputId);
            }

            player.movable.MoveByPlayerInput(player.controllable.playerInput);
        }

        public void EnqueueInput(int inputId, PlayerInput playerInput)
        {
            queuedInputs.Enqueue(new Tuple<int, PlayerInput>(inputId, playerInput));
        }
    }
}
