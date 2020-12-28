using System;
using System.Collections.Generic;
using AmongUsClone.Server.Snapshots;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    [RequireComponent(typeof(Player))]
    public class RemoteControllable : MonoBehaviour
    {
        private Player player;

        private readonly Queue<PlayerInput> queuedInputs = new Queue<PlayerInput>();
        public int lastProcessedInputId;

        private void Start()
        {
            player = GetComponent<Player>();
        }

        public void FixedUpdate()
        {
            player.controllable.UpdateInput(GetPlayerInput());
            player.movable.MoveByPlayerInput(player.controllable.playerInput);
        }

        private PlayerInput GetPlayerInput()
        {
            // Sometimes packets may be lost, then we think that player was standing still
            if (queuedInputs.Count == 0)
            {
                return new PlayerInput();
            }

            PlayerInput playerInput = queuedInputs.Dequeue();
            UpdateLastProcessedInputId(playerInput);

            return playerInput;
        }

        private void UpdateLastProcessedInputId(PlayerInput playerInput)
        {
            if (lastProcessedInputId >= playerInput.id)
            {
                Logger.LogError(LoggerSection.GameSnapshots, $"Got less or equal last inputId. Last remembered: {lastProcessedInputId}, new: {playerInput.id}");
                return;
            }

            lastProcessedInputId = playerInput.id;
        }

        public void EnqueueInput(PlayerInput playerInput)
        {
            queuedInputs.Enqueue(playerInput);
        }
    }
}
