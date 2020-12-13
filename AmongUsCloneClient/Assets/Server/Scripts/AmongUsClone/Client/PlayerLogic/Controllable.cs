using System.Collections;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    [RequireComponent(typeof(Movable))]
    public class Controllable : MonoBehaviour
    {
        private readonly PlayerInput playerInput = new PlayerInput();
        private Movable movable;

        private void Awake()
        {
            movable = GetComponent<Movable>();
        }

        // Todo: consider migrating to Update
        private void FixedUpdate()
        {
            UpdatePlayerInput();
            StartCoroutine(SendInputToServer(playerInput.Clone()));

            if (NetworkingOptimizationTests.isPredictionEnabled)
            {
                movable.MoveByPlayerInput(playerInput);
            }
        }

        private void UpdatePlayerInput()
        {
            playerInput.moveTop = Input.GetKey(KeyCode.W);
            playerInput.moveLeft = Input.GetKey(KeyCode.A);
            playerInput.moveBottom = Input.GetKey(KeyCode.S);
            playerInput.moveRight = Input.GetKey(KeyCode.D);
        }

        private static IEnumerator SendInputToServer(PlayerInput playerInput)
        {
            float secondsToWait = NetworkingOptimizationTests.millisecondsLag * 0.001f;
            yield return new WaitForSeconds(secondsToWait);

            PacketsSender.SendPlayerInputPacket(playerInput);
        }

    }
}
