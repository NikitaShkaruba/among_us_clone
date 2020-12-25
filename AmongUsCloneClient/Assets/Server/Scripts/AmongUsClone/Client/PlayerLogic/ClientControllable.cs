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

        private int controlsRequestId;
        public readonly Dictionary<int, PlayerControls> cachedSentToServerControls = new Dictionary<int, PlayerControls>();

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void FixedUpdate()
        {
            UpdatePlayerControls();

            if (HasNewInputsForServer())
            {
                StartCoroutine(SendControlsToServer(player.controllable.playerControls.Clone()));
            }

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

        private bool HasNewInputsForServer()
        {
            return player.controllable.playerControls.moveTop ||
                   player.controllable.playerControls.moveLeft ||
                   player.controllable.playerControls.moveRight ||
                   player.controllable.playerControls.moveBottom;
        }

        private IEnumerator SendControlsToServer(PlayerControls playerControls)
        {
            controlsRequestId++;
            cachedSentToServerControls[controlsRequestId] = playerControls;

            // Simulate network lag
            float secondsToWait = NetworkingOptimizationTests.millisecondsLag * 0.001f;
            yield return new WaitForSeconds(secondsToWait);

            PacketsSender.SendPlayerControlsPacket(controlsRequestId, playerControls);
        }

        public void RemoveObsoleteRequests(GameSnapshot gameSnapshot)
        {
            // No snapshots to remove
            if (cachedSentToServerControls.Keys.Count == 0)
            {
                return;
            }

            int minRequestId = cachedSentToServerControls.Keys.Min();
            int maxRequestId = cachedSentToServerControls.Keys.Max();

            for (int requestId = minRequestId; requestId <= maxRequestId; requestId++)
            {
                if (requestId <= gameSnapshot.lastControlsRequestId)
                {
                    cachedSentToServerControls.Remove(requestId);
                }
            }
        }
    }
}
