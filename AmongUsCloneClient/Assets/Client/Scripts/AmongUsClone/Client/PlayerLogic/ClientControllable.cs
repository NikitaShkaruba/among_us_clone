using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    [RequireComponent(typeof(Player))]
    public class ClientControllable : MonoBehaviour
    {
        private Player player;

        private int inputId;
        public readonly Dictionary<int, ClientControllableStateSnapshot> stateSnapshots = new Dictionary<int, ClientControllableStateSnapshot>();

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void FixedUpdate()
        {
            UpdatePlayerInput();

            if (NetworkingOptimizationTests.isPredictionEnabled)
            {
                player.movable.MoveByPlayerInput(player.controllable.playerInput);
            }

            StartCoroutine(SendInputToServer(player.movable.transform.position, player.controllable.playerInput.Clone()));
        }

        private void UpdatePlayerInput()
        {
            inputId++;

            PlayerInput newPlayerInput = new PlayerInput
            {
                id = inputId,
                moveTop = Input.GetKey(KeyCode.W),
                moveLeft = Input.GetKey(KeyCode.A),
                moveBottom = Input.GetKey(KeyCode.S),
                moveRight = Input.GetKey(KeyCode.D),
            };

            player.controllable.UpdateInput(newPlayerInput);
        }

        private IEnumerator SendInputToServer(Vector2 playerPosition, PlayerInput playerInput)
        {
            stateSnapshots[playerInput.id] = new ClientControllableStateSnapshot(playerInput, playerPosition);

            // Simulate network lag
            float secondsToWait = NetworkingOptimizationTests.NetworkDelayInSeconds;
            yield return new WaitForSeconds(secondsToWait);

            PacketsSender.SendPlayerInputPacket(playerInput);
        }

        public void RemoveObsoleteStates(GameSnapshot gameSnapshot)
        {
            // No snapshots to remove
            if (stateSnapshots.Count == 0)
            {
                return;
            }

            for (int inputId = stateSnapshots.Keys.Min(); inputId < gameSnapshot.yourLastProcessedInputId; inputId++)
            {
                stateSnapshots.Remove(inputId);
            }
        }

        public void UpdatePositionHistory(int inputId, Vector2 newPosition)
        {
            stateSnapshots[inputId].position = newPosition;
        }
    }
}
