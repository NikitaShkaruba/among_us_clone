using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    [RequireComponent(typeof(Player))]
    public class ClientControllable : MonoBehaviour
    {
        private Player player;

        public readonly Dictionary<int, ClientControllableStateSnapshot> stateSnapshots = new Dictionary<int, ClientControllableStateSnapshot>();

        private void Awake()
        {
            player = GetComponent<Player>();
            GameManager.instance.RegisterControlledPlayer(player);
        }

        private void FixedUpdate()
        {
            UpdatePlayerInput();
            player.movable.MoveByPlayerInput(player.controllable.playerInput);

            StartCoroutine(SaveStateSnapshot());
            PacketsSender.SendPlayerInputPacket(player.controllable.playerInput.Clone());
        }

        private IEnumerator SaveStateSnapshot()
        {
            yield return new WaitForFixedUpdate();

            stateSnapshots[player.controllable.playerInput.id] = new ClientControllableStateSnapshot(player.controllable.playerInput, player.movable.transform.position);
        }

        private void UpdatePlayerInput()
        {
            player.controllable.playerInput = new PlayerInput
            {
                id = GenerateNextPlayerInputId(),
                moveTop = Input.GetKey(KeyCode.W),
                moveLeft = Input.GetKey(KeyCode.A),
                moveBottom = Input.GetKey(KeyCode.S),
                moveRight = Input.GetKey(KeyCode.D),
            };
        }

        public void RemoveObsoleteSnapshotStates(ClientGameSnapshot gameSnapshot)
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

        public void UpdateSnapshotStatePosition(int inputId, Vector2 newPosition)
        {
            stateSnapshots[inputId].position = newPosition;
        }

        private int GenerateNextPlayerInputId()
        {
            if (stateSnapshots.Count == 0)
            {
                return 0;
            }

            return stateSnapshots.Keys.Max() + 1;
        }
    }
}
