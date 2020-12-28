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
            // Todo: fix always playerId being always 1
            // Todo: remove lobby call
            ClientControllable clientControllable = GameManager.instance.lobby.players[0].GetComponent<ClientControllable>();
            clientControllable.RemoveObsoleteSnapshotStates(gameSnapshot);

            foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo)
            {
                if (NetworkingOptimizationTests.isReconciliationEnabled)
                {
                    if (IsReconciliationNeeded(gameSnapshot))
                    {
                        Reconcile(gameSnapshot);
                    }
                }
                else
                {
                    GameManager.instance.UpdatePlayerPosition(snapshotPlayerInfo.id, snapshotPlayerInfo.position);
                }
            }

            // Todo: remove debug
            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {gameSnapshot.id}");
            GameSnapshotsDebug.Log(gameSnapshot, GameManager.instance.lobby.players[0]);
        }

        private static bool IsReconciliationNeeded(ClientGameSnapshot gameSnapshot)
        {
            // Todo: support multiple players
            const float acceptablePositionError = 0.0000001f;
            ClientControllable clientControllable = GameManager.instance.lobby.players[0].GetComponent<ClientControllable>();

            Vector2 serverPosition = gameSnapshot.playersInfo[0].position;
            Vector2 clientPosition = clientControllable.stateSnapshots[gameSnapshot.yourLastProcessedInputId].position;
            Vector2 positionDifference = serverPosition - clientPosition;

            return positionDifference.sqrMagnitude > acceptablePositionError;
        }

        private static void Reconcile(ClientGameSnapshot gameSnapshot)
        {
            // Todo: support multiple players
            Player player = GameManager.instance.lobby.players[0];
            ClientControllable clientControllable = player.GetComponent<ClientControllable>();

            Logger.LogDebug($"Reconciling with the server. YourLastProcessedInputId: {gameSnapshot.yourLastProcessedInputId}. Server position: {gameSnapshot.playersInfo[0].position}. Client position: {clientControllable.stateSnapshots[gameSnapshot.yourLastProcessedInputId]}.");

            // Teleport to server location
            player.movable.Move(gameSnapshot.playersInfo[0].position);
            clientControllable.UpdateSnapshotState(gameSnapshot.yourLastProcessedInputId, gameSnapshot.playersInfo[0].position);
            Logger.LogDebug($"Teleported player by reconciliation. To {gameSnapshot.playersInfo[0].position}");

            // Apply not yet processed by server inputs
            for (int inputId = gameSnapshot.yourLastProcessedInputId + 1; inputId <= clientControllable.stateSnapshots.Keys.Max(); inputId++)
            {
                Vector2 newPosition = player.movable.MoveByPlayerInput(clientControllable.stateSnapshots[inputId].input);
                clientControllable.UpdateSnapshotState(inputId, newPosition);
            }
        }
    }
}
