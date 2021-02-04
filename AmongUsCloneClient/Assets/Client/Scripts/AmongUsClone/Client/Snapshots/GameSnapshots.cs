using System.Linq;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.PlayerLogic;
using AmongUsClone.Shared.Scenes;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;
using Scene = AmongUsClone.Client.Game.Scene;

namespace AmongUsClone.Client.Snapshots
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "GameSnapshots", menuName = "GameSnapshots")]
    public class GameSnapshots : ScriptableObject
    {
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private PlayersManager playersManager;

        public void ProcessSnapshot(ClientGameSnapshot gameSnapshot)
        {
            if (!ShouldProcessServerSnapshots())
            {
                return;
            }

            UpdatePlayers(gameSnapshot);

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {gameSnapshot}");
        }

        private void UpdatePlayers(ClientGameSnapshot gameSnapshot)
        {
            foreach (Player player in playersManager.players.Values)
            {
                if (player == playersManager.controlledPlayer)
                {
                    UpdateControlledPlayer(gameSnapshot, player);
                }
                else if (gameSnapshot.playersInfo.ContainsKey(player.information.id))
                {
                    if (!player.gameObject.activeSelf)
                    {
                        // Todo: fix slow 'just becoming visible' players
                        player.gameObject.SetActive(true);
                    }

                    UpdateNotControlledPlayer(gameSnapshot.playersInfo[player.information.id]);
                }
                else
                {
                    if (player.gameObject.activeSelf)
                    {
                        player.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void UpdateNotControlledPlayer(SnapshotPlayerInfo snapshotPlayerInfo)
        {
            // If we don't have disconnected player anymore
            if (!playersManager.players.ContainsKey(snapshotPlayerInfo.id))
            {
                return;
            }

            playersManager.UpdatePlayerWithServerState(snapshotPlayerInfo.id, snapshotPlayerInfo.position, snapshotPlayerInfo.input);
        }

        private void UpdateControlledPlayer(ClientGameSnapshot gameSnapshot, Player controlledPlayer)
        {
            controlledPlayer.clientControllable.RemoveObsoleteSnapshotStates(gameSnapshot);

            if (IsReconciliationNeeded(controlledPlayer, gameSnapshot))
            {
                Reconcile(controlledPlayer, gameSnapshot);
            }
        }

        private bool IsReconciliationNeeded(Player controlledPlayer, ClientGameSnapshot gameSnapshot)
        {
            // If player has just spawned, he might not have gameSnapshots with which he may reconcile
            if (!controlledPlayer.clientControllable.stateSnapshots.ContainsKey(gameSnapshot.yourLastProcessedInputId))
            {
                return false;
            }

            const float acceptablePositionError = 0.01f;

            Vector2 serverPosition = gameSnapshot.playersInfo[controlledPlayer.information.id].position;
            Vector2 clientPosition = controlledPlayer.clientControllable.stateSnapshots[gameSnapshot.yourLastProcessedInputId].position;
            Vector2 positionDifference = serverPosition - clientPosition;

            return positionDifference.magnitude > acceptablePositionError;
        }

        private void Reconcile(Player controlledPlayer, ClientGameSnapshot gameSnapshot)
        {
            ClientControllable clientControllable = controlledPlayer.clientControllable;
            Vector2 incorrectClientPosition = clientControllable.stateSnapshots[gameSnapshot.yourLastProcessedInputId].position;
            Vector2 correctServerPosition = gameSnapshot.playersInfo[controlledPlayer.information.id].position;

            Physics2D.simulationMode = SimulationMode2D.Script;

            // Teleport to server location
            controlledPlayer.movable.Teleport(correctServerPosition);
            Physics2D.Simulate(Time.fixedDeltaTime);
            clientControllable.UpdateSnapshotStatePosition(gameSnapshot.yourLastProcessedInputId, controlledPlayer.transform.position);

            // Apply not yet processed by server inputs
            for (int inputId = gameSnapshot.yourLastProcessedInputId + 1; inputId <= clientControllable.stateSnapshots.Keys.Max(); inputId++)
            {
                controlledPlayer.movable.MoveByPlayerInput(clientControllable.stateSnapshots[inputId].input);
                Physics2D.Simulate(Time.fixedDeltaTime);
                clientControllable.UpdateSnapshotStatePosition(inputId, controlledPlayer.transform.position);
            }

            Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

            Logger.LogEvent(LoggerSection.ServerReconciliation, $"Reconciled position with the server position. SnapshotId: {gameSnapshot.id}. YourLastProcessedInputId: {gameSnapshot.yourLastProcessedInputId}. Server position: {correctServerPosition}. Client position: {incorrectClientPosition}.");
        }

        private bool ShouldProcessServerSnapshots()
        {
            string[] activeScenes =
            {
                Scene.Lobby,
                Scene.Skeld
            };

            return activeScenes.Contains(scenesManager.GetActiveScene());
        }
    }
}
