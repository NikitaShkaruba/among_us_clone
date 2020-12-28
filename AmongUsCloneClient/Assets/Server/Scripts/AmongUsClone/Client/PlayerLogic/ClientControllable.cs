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

        // Todo: merge into one structure
        private int inputId;
        public readonly Dictionary<int, PlayerInput> snapshotsInputs = new Dictionary<int, PlayerInput>();
        public readonly Dictionary<int, Vector2> snapshotsPositions = new Dictionary<int, Vector2>();

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

            inputId++;
            StartCoroutine(SendInputToServer(inputId, player.movable.transform.position, player.controllable.playerInput.Clone()));
        }

        private void UpdatePlayerInput()
        {
            PlayerInput newPlayerInput = new PlayerInput
            {
                moveTop = Input.GetKey(KeyCode.W),
                moveLeft = Input.GetKey(KeyCode.A),
                moveBottom = Input.GetKey(KeyCode.S),
                moveRight = Input.GetKey(KeyCode.D),
            };

            player.controllable.UpdateInput(newPlayerInput);
        }

        private IEnumerator SendInputToServer(int inputId, Vector2 playerPosition, PlayerInput playerInput)
        {
            snapshotsInputs[inputId] = playerInput;
            snapshotsPositions[inputId] = playerPosition;

            // Simulate network lag
            float secondsToWait = NetworkingOptimizationTests.NetworkDelayInSeconds;
            yield return new WaitForSeconds(secondsToWait);

            PacketsSender.SendPlayerInputPacket(inputId, playerInput);
        }

        public void RemoveObsoleteStates(GameSnapshot gameSnapshot)
        {
            // No snapshots to remove
            if (snapshotsInputs.Count == 0)
            {
                return;
            }

            for (int inputId = snapshotsInputs.Keys.Min(); inputId < gameSnapshot.yourLastProcessedInputId; inputId++)
            {
                snapshotsInputs.Remove(inputId);
                snapshotsPositions.Remove(inputId);
            }
        }

        public void UpdatePositionHistory(int inputId, Vector2 newPosition)
        {
            snapshotsPositions[inputId] = newPosition;
        }
    }
}
