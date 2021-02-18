using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using PlayerInput = AmongUsClone.Shared.Game.PlayerLogic.PlayerInput;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    public class ClientControllable : MonoBehaviour
    {
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private InputReader inputReader;

        [SerializeField] private ClientPlayer clientPlayer;

        private Vector2 movement;
        public bool Interact { get; private set; }
        public bool StartGame { get; private set; }

        public readonly Dictionary<int, ClientControllableStateSnapshot> stateSnapshots = new Dictionary<int, ClientControllableStateSnapshot>();

        private void Awake()
        {
            clientPlayer = GetComponent<ClientPlayer>();
        }

        private void OnEnable() {
            inputReader.onMove += OnMovement;
            inputReader.onInteract += OnInteract;
        }

        private void OnDisable()
        {
            inputReader.onMove -= OnMovement;
            inputReader.onInteract -= OnInteract;
        }

        private void OnMovement(Vector2 movement)
        {
            this.movement = movement;
        }

        public void OnInteract()
        {
            Interact = true;
        }

        public void OnStartGame()
        {
            StartGame = true;
        }

        private void FixedUpdate()
        {
            UpdatePlayerInput();

            clientPlayer.basePlayer.movable.MoveByPlayerInput(clientPlayer.basePlayer.controllable.playerInput);

            packetsSender.SendPlayerInputPacket(clientPlayer.basePlayer.controllable.playerInput.Clone());
            StartCoroutine(LateFixedUpdate());
        }

        private IEnumerator LateFixedUpdate()
        {
            yield return new WaitForFixedUpdate();

            Interact = false;
            StartGame = false;
            stateSnapshots[clientPlayer.basePlayer.controllable.playerInput.id] = new ClientControllableStateSnapshot(clientPlayer.basePlayer.controllable.playerInput, clientPlayer.basePlayer.movable.transform.position);
        }

        private void UpdatePlayerInput()
        {
            clientPlayer.basePlayer.controllable.playerInput = new PlayerInput
            {
                id = GenerateNextPlayerInputId(),
                moveTop = movement.y > 0,
                moveLeft = movement.x < 0,
                moveBottom = movement.y < 0,
                moveRight = movement.x > 0,
                interact = Interact,
                startGame = StartGame,
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
