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
        public static void ProcessSnapshot(GameSnapshot gameSnapshot)
        {
            // Todo: fix always playerId being always 1
            ClientControllable clientControllable = GameManager.instance.lobby.players[0].GetComponent<ClientControllable>();
            clientControllable.RemoveObsoleteStates(gameSnapshot);

            foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo)
            {
                if (NetworkingOptimizationTests.isReconciliationEnabled)
                {
                    if (IsReconciliationNeeded(gameSnapshot))
                    {
                        Reconciliate(gameSnapshot);
                    }
                }
                else
                {
                    GameManager.instance.UpdatePlayerPosition(snapshotPlayerInfo.id, snapshotPlayerInfo.position);
                }
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {gameSnapshot.id}");
            GameSnapshotsDebug.Log(gameSnapshot, GameManager.instance.lobby.players[0]);
        }

        private static bool IsReconciliationNeeded(GameSnapshot gameSnapshot)
        {
            return false;

            const float acceptablePositionError = 0.0000001f;
            ClientControllable clientControllable = GameManager.instance.lobby.players[0].GetComponent<ClientControllable>();

            Vector2 serverPosition = gameSnapshot.playersInfo[0].position;
            Vector2 clientPosition = clientControllable.snapshotsPositions[gameSnapshot.yourLastProcessedInputId + 1];

            if (!Mathf.Approximately(serverPosition.x, 0) || !Mathf.Approximately(serverPosition.y, 0))
            {
                // Logger.LogDebug($"Reconciliation case. Should have all the information to reconcile");
            }

            Vector2 positionDifference = serverPosition - clientPosition;
            bool isReconciliationNeeded = positionDifference.sqrMagnitude > acceptablePositionError;
            if (isReconciliationNeeded)
            {
                // Logger.LogDebug($"Reconciliation happened. Snapshot: {gameSnapshot.id}. LastControlsId: {gameSnapshot.lastControlsRequestId}.");
            }

            return isReconciliationNeeded;
        }

        private static void Reconciliate(GameSnapshot gameSnapshot)
        {
            return;

            Player player = GameManager.instance.lobby.players[0];
            ClientControllable clientControllable = player.GetComponent<ClientControllable>();
            Vector2 serverPosition = gameSnapshot.playersInfo[0].position;

            // Physics.autoSimulation = false;

            player.movable.Move(serverPosition);
            // Physics.Simulate(Time.fixedDeltaTime);

            for (int controlsRequestId = gameSnapshot.yourLastProcessedInputId + 1; controlsRequestId < clientControllable.snapshotsPositions.Keys.Max(); controlsRequestId++)
            {
                // Vector2 clientPosition = clientControllable.snapshotsPositions[];
                player.movable.MoveByPlayerInput(clientControllable.snapshotsInputs[controlsRequestId]);
                Logger.LogDebug($"Reconciled controls request {controlsRequestId}. Player position: {player.movable.rigidbody.position}.");
                // Physics.Simulate(Time.fixedDeltaTime);
            }

            // Physics.autoSimulation = true;
        }
    }
}
