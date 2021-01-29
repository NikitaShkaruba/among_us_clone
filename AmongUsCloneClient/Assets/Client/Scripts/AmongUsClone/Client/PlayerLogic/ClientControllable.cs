using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using PlayerInput = AmongUsClone.Shared.Game.PlayerLogic.PlayerInput;

namespace AmongUsClone.Client.PlayerLogic
{
    public class ClientControllable : MonoBehaviour
    {
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private InputReader inputReader;

        [SerializeField] private Player player;

        private Vector2 movement;

        public readonly Dictionary<int, ClientControllableStateSnapshot> stateSnapshots = new Dictionary<int, ClientControllableStateSnapshot>();

        private void Awake()
        {
            player = GetComponent<Player>();
        }

        private void OnEnable() {
            inputReader.onMove += OnMovement;
        }

        private void OnDisable()
        {
            inputReader.onMove -= OnMovement;
        }

        private void OnMovement(Vector2 movement)
        {
            this.movement = movement;
        }

        private void FixedUpdate()
        {
            UpdatePlayerInput();

            player.movable.MoveByPlayerInput(player.controllable.playerInput);

            StartCoroutine(SaveStateSnapshot());
            packetsSender.SendPlayerInputPacket(player.controllable.playerInput.Clone());
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
                moveTop = movement.y > 0,
                moveLeft = movement.x < 0,
                moveBottom = movement.y < 0,
                moveRight = movement.x > 0,
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
