using System.Linq;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Logging;
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
        [SerializeField] private PlayGamePhase playGamePhase;

        public void ProcessSnapshot(ClientGameSnapshot gameSnapshot)
        {
            if (!ShouldProcessServerSnapshots())
            {
                return;
            }

            UpdatePlayers(gameSnapshot);

            if (gameSnapshot.adminPanelPositions.Count != 0)
            {
                playGamePhase.UpdateAdminPanelMinimap(gameSnapshot.adminPanelPositions);
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {gameSnapshot}");
        }

        private void UpdatePlayers(ClientGameSnapshot gameSnapshot)
        {
            foreach (ClientPlayer player in playersManager.players.Values)
            {
                if (player == playersManager.controlledClientPlayer.clientPlayer)
                {
                    UpdateControlledPlayer(gameSnapshot, playersManager.controlledClientPlayer);
                }
                else if (gameSnapshot.playersInfo.ContainsKey(player.basePlayer.information.id))
                {
                    if (!player.gameObject.activeSelf)
                    {
                        // Todo: implement some kind of client anti-cheat (It is impossible to have a clear solution, check out my reddit question about it)
                        // @link https://www.reddit.com/r/gamedev/comments/lcq93c/how_to_use_clientside_prediction_with_fog_of_war/
                        player.gameObject.SetActive(true);
                    }

                    UpdateNotControlledPlayer(gameSnapshot.playersInfo[player.basePlayer.information.id]);
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

        private void UpdateControlledPlayer(ClientGameSnapshot gameSnapshot, ClientControllablePlayer controlledClientPlayer)
        {
            controlledClientPlayer.clientControllable.RemoveObsoleteSnapshotStates(gameSnapshot);

            if (IsReconciliationNeeded(controlledClientPlayer, gameSnapshot))
            {
                Reconcile(controlledClientPlayer, gameSnapshot);
            }
        }

        private bool IsReconciliationNeeded(ClientControllablePlayer controlledClientPlayer, ClientGameSnapshot gameSnapshot)
        {
            // If player has just spawned, he might not have gameSnapshots with which he may reconcile
            if (!controlledClientPlayer.clientControllable.stateSnapshots.ContainsKey(gameSnapshot.yourLastProcessedInputId))
            {
                return false;
            }

            const float acceptablePositionError = 0.01f;

            Vector2 serverPosition = gameSnapshot.playersInfo[controlledClientPlayer.basePlayer.information.id].position;
            Vector2 clientPosition = controlledClientPlayer.clientControllable.stateSnapshots[gameSnapshot.yourLastProcessedInputId].position;
            Vector2 positionDifference = serverPosition - clientPosition;

            return positionDifference.magnitude > acceptablePositionError;
        }

        private void Reconcile(ClientControllablePlayer clientControllablePlayer, ClientGameSnapshot gameSnapshot)
        {
            ClientControllable clientControllable = clientControllablePlayer.clientControllable;
            Vector2 incorrectClientPosition = clientControllable.stateSnapshots[gameSnapshot.yourLastProcessedInputId].position;
            Vector2 correctServerPosition = gameSnapshot.playersInfo[clientControllablePlayer.basePlayer.information.id].position;

            Physics2D.simulationMode = SimulationMode2D.Script;

            // Teleport to server location
            clientControllablePlayer.basePlayer.movable.Teleport(correctServerPosition);
            Physics2D.Simulate(Time.fixedDeltaTime);
            clientControllable.UpdateSnapshotStatePosition(gameSnapshot.yourLastProcessedInputId, clientControllablePlayer.transform.position);

            // Apply not yet processed by server inputs
            for (int inputId = gameSnapshot.yourLastProcessedInputId + 1; inputId <= clientControllable.stateSnapshots.Keys.Max(); inputId++)
            {
                clientControllablePlayer.basePlayer.movable.MoveByPlayerInput(clientControllable.stateSnapshots[inputId].input);
                Physics2D.Simulate(Time.fixedDeltaTime);
                clientControllable.UpdateSnapshotStatePosition(inputId, clientControllablePlayer.transform.position);
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
