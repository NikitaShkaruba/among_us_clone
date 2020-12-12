using System.Collections;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared;
using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    [RequireComponent(typeof(Movable))]
    public class Controllable : MonoBehaviour
    {
        // Todo: move to shared
        private const float MoveSpeed = 0.2f;

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
                MovePlayer();
            }
        }

        private void UpdatePlayerInput()
        {
            playerInput.moveTop = Input.GetKey(KeyCode.W);
            playerInput.moveLeft = Input.GetKey(KeyCode.A);
            playerInput.moveBottom = Input.GetKey(KeyCode.S);
            playerInput.moveRight = Input.GetKey(KeyCode.D);
        }

        private void MovePlayer()
        {
            Vector2 moveDirection = GetMoveDirection();

            Vector2 relativePosition = moveDirection * MoveSpeed;
            movable.RelativeMove(relativePosition);
        }

        // Todo: refactor into shared
        private Vector2 GetMoveDirection()
        {
            Vector2 moveDirection = new Vector2(0f, 0f);

            if (playerInput.moveTop)
            {
                moveDirection.y++;
            }

            if (playerInput.moveLeft)
            {
                moveDirection.x--;
            }

            if (playerInput.moveBottom)
            {
                moveDirection.y--;
            }

            if (playerInput.moveRight)
            {
                moveDirection.x++;
            }

            return moveDirection;
        }


        private IEnumerator SendInputToServer(PlayerInput playerInput)
        {
            float secondsToWait = NetworkingOptimizationTests.millisecondsLag * 0.001f;
            yield return new WaitForSeconds(secondsToWait);

            PacketsSender.SendPlayerInputPacket(playerInput);
        }

    }
}
