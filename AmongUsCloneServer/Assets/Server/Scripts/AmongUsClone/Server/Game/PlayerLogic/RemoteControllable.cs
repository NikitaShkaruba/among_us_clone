using System.Collections.Generic;
using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    [RequireComponent(typeof(ServerPlayer))]
    public class RemoteControllable : MonoBehaviour
    {
        [SerializeField] private LobbyGamePhase lobbyGamePhase;

        [SerializeField] private ServerPlayer serverPlayer;
        public bool isLobbyHost;

        private readonly Queue<PlayerInput> queuedInputs = new Queue<PlayerInput>();
        public int lastProcessedInputId = -1;

        public void FixedUpdate()
        {
            serverPlayer.basePlayer.controllable.playerInput = GetPlayerInput();

            serverPlayer.basePlayer.movable.MoveByPlayerInput(serverPlayer.basePlayer.controllable.playerInput);

            if (serverPlayer.basePlayer.controllable.playerInput.interact && serverPlayer.nearbyInteractableChooser.chosen)
            {
                serverPlayer.nearbyInteractableChooser.chosen.Interact(serverPlayer.basePlayer.information.id);
            }

            if (serverPlayer.basePlayer.controllable.playerInput.startGame)
            {
                lobbyGamePhase.TryToScheduleGameStart(serverPlayer.basePlayer.information.id);
            }
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
