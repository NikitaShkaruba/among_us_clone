using System;
using System.Collections;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.PlayerLogic
{
    [RequireComponent(typeof(Player))]
    public class ClientControllable : MonoBehaviour
    {
        private Player player;

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void FixedUpdate()
        {
            UpdatePlayerControls();
            StartCoroutine(SendControlsToServer(player.controllable.playerControls.Clone()));

            if (NetworkingOptimizationTests.isPredictionEnabled)
            {
                player.movable.MoveByPlayerControls(player.controllable.playerControls);
            }
        }

        private void UpdatePlayerControls()
        {
            PlayerControls newPlayerControls = new PlayerControls()
            {
                moveTop = Input.GetKey(KeyCode.W),
                moveLeft = Input.GetKey(KeyCode.A),
                moveBottom = Input.GetKey(KeyCode.S),
                moveRight = Input.GetKey(KeyCode.D),
            };

            player.controllable.UpdateControls(newPlayerControls);
        }

        private static IEnumerator SendControlsToServer(PlayerControls playerControls)
        {
            float secondsToWait = NetworkingOptimizationTests.millisecondsLag * 0.001f;
            yield return new WaitForSeconds(secondsToWait);

            PacketsSender.SendPlayerControlsPacket(playerControls);
        }
    }
}
