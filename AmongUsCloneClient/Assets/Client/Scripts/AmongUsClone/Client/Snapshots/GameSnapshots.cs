using System.Linq;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.PlayerLogic;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Snapshots
{
    public static class GameSnapshots
    {
        public static void ProcessSnapshot(ClientGameSnapshot gameSnapshot)
        {
            UpdatePlayers(gameSnapshot);

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {gameSnapshot}");
            // Todo: remove debug
            GameSnapshotsDebug.Log(gameSnapshot, GameManager.instance.controlledPlayer);
        }

        private static void UpdatePlayers(ClientGameSnapshot gameSnapshot)
        {
            Player controlledPlayer = GameManager.instance.controlledPlayer;

            foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo.Values)
            {
                if (snapshotPlayerInfo.id == controlledPlayer.id)
                {
                    UpdateControlledPlayer(gameSnapshot, controlledPlayer);
                }
                else
                {
                    UpdateNotControlledPlayer(snapshotPlayerInfo);
                }
            }
        }

        private static void UpdateNotControlledPlayer(SnapshotPlayerInfo snapshotPlayerInfo)
        {
            GameManager.instance.UpdatePlayerPosition(snapshotPlayerInfo.id, snapshotPlayerInfo.position);
        }

        private static void UpdateControlledPlayer(ClientGameSnapshot gameSnapshot, Player controlledPlayer)
        {
            controlledPlayer.GetComponent<ClientControllable>().RemoveObsoleteSnapshotStates(gameSnapshot);

            if (NetworkingOptimizationTests.isReconciliationEnabled)
            {
                if (IsReconciliationNeeded(controlledPlayer, gameSnapshot))
                {
                    Reconcile(controlledPlayer, gameSnapshot);
                }
            }
            else
            {
                GameManager.instance.UpdatePlayerPosition(controlledPlayer.id, gameSnapshot.playersInfo[controlledPlayer.id].position);
            }
        }

        private static bool IsReconciliationNeeded(Player controlledPlayer, ClientGameSnapshot gameSnapshot)
        {
            const float acceptablePositionError = 0.0000001f;

            Vector2 serverPosition = gameSnapshot.playersInfo[controlledPlayer.id].position;
            Vector2 clientPosition = controlledPlayer.GetComponent<ClientControllable>().stateSnapshots[gameSnapshot.yourLastProcessedInputId].position;
            Vector2 positionDifference = serverPosition - clientPosition;

            return positionDifference.sqrMagnitude > acceptablePositionError;
        }

        private static void Reconcile(Player controlledPlayer, ClientGameSnapshot gameSnapshot)
        {
            ClientControllable clientControllable = controlledPlayer.GetComponent<ClientControllable>();
            Vector2 correctServerPosition = gameSnapshot.playersInfo[controlledPlayer.id].position;

            // Todo: add proper logs for this section
            Logger.LogDebug($"Reconciling with the server. YourLastProcessedInputId: {gameSnapshot.yourLastProcessedInputId}. Server position: {correctServerPosition}. Client position: {clientControllable.stateSnapshots[gameSnapshot.yourLastProcessedInputId]}.");

            // Teleport to server location
            controlledPlayer.movable.Move(correctServerPosition);
            clientControllable.UpdateSnapshotState(gameSnapshot.yourLastProcessedInputId, correctServerPosition);

            // Apply not yet processed by server inputs
            for (int inputId = gameSnapshot.yourLastProcessedInputId + 1; inputId <= clientControllable.stateSnapshots.Keys.Max(); inputId++)
            {
                Vector2 correctlyPredictedPosition = controlledPlayer.movable.MoveByPlayerInput(clientControllable.stateSnapshots[inputId].input);
                clientControllable.UpdateSnapshotState(inputId, correctlyPredictedPosition);
            }
        }
    }
}
