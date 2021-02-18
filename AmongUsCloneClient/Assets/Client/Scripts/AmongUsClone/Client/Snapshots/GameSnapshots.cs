using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Game.Maps.Surveillance;
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
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private PlayGamePhase playGamePhase;

        public void ProcessSnapshot(ClientGameSnapshot gameSnapshot)
        {
            ProcessPlayersData(gameSnapshot);
            ProcessLobbyGamePhaseData(gameSnapshot);
            ProcessPlayGamePhaseData(gameSnapshot);

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {gameSnapshot}");
        }

        private void ProcessLobbyGamePhaseData(ClientGameSnapshot gameSnapshot)
        {
            if (lobbyGamePhase.lobby == null)
            {
                return;
            }

            if (gameSnapshot.gameStarts && !lobbyGamePhase.IsGameStarting)
            {
                lobbyGamePhase.InitiateGameStart();
            }

            if (gameSnapshot.gameStarted && !lobbyGamePhase.IsGameStarted)
            {
                List<int> impostorPlayerIds = new List<int>();

                bool isPlayingAsImpostor = gameSnapshot.playersInfo[playersManager.controlledClientPlayer.basePlayer.information.id].isImpostor;
                if (isPlayingAsImpostor)
                {
                    foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo.Values)
                    {
                        if (snapshotPlayerInfo.isImpostor)
                        {
                            impostorPlayerIds.Add(snapshotPlayerInfo.id);
                        }
                    }
                }

                lobbyGamePhase.StartGame(isPlayingAsImpostor, gameSnapshot.impostorsAmount, impostorPlayerIds.ToArray());
            }
        }

        private void ProcessPlayGamePhaseData(ClientGameSnapshot gameSnapshot)
        {
            if (playGamePhase.clientSkeld == null)
            {
                return;
            }

            if (gameSnapshot.adminPanelPositions.Count != 0)
            {
                playGamePhase.UpdateAdminPanelMinimap(gameSnapshot.adminPanelPositions);
            }

            SecurityPanel securityPanel = playGamePhase.clientSkeld.securityPanel;
            if (gameSnapshot.securityCamerasEnabled && !securityPanel.CamerasEnabled)
            {
                securityPanel.EnableSecurityCameras();
            }
            else if (!gameSnapshot.securityCamerasEnabled && securityPanel.CamerasEnabled)
            {
                securityPanel.DisableSecurityCameras();
            }
        }

        private void ProcessPlayersData(ClientGameSnapshot gameSnapshot)
        {
            AddNewlyConnectedPlayers(gameSnapshot);
            UpdateExistentPlayers(gameSnapshot);
            RemoveDisconnectedPlayers(gameSnapshot);
        }

        private void AddNewlyConnectedPlayers(ClientGameSnapshot gameSnapshot)
        {
            if (!mainMenuGamePhase.connectionToServer.IsConnected)
            {
                return;
            }

            foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo.Values)
            {
                if (playersManager.players.ContainsKey(snapshotPlayerInfo.id))
                {
                    continue;
                }

                if (scenesManager.GetActiveScene() == Scene.MainMenu)
                {
                    mainMenuGamePhase.InitializeLobby(snapshotPlayerInfo.id, snapshotPlayerInfo.name, snapshotPlayerInfo.color, snapshotPlayerInfo.position, snapshotPlayerInfo.isLobbyHost);
                }
                else
                {
                    lobbyGamePhase.AddPlayerToLobby(snapshotPlayerInfo.id, snapshotPlayerInfo.name, snapshotPlayerInfo.color, snapshotPlayerInfo.position, snapshotPlayerInfo.isLobbyHost);
                }
            }
        }

        private void UpdateExistentPlayers(ClientGameSnapshot gameSnapshot)
        {
            if (!ShouldProcessServerSnapshots())
            {
                return;
            }

            foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo.Values)
            {
                if (!playersManager.players.ContainsKey(snapshotPlayerInfo.id))
                {
                    continue;
                }

                ClientPlayer clientPlayer = playersManager.players[snapshotPlayerInfo.id];

                if (clientPlayer == playersManager.controlledClientPlayer.clientPlayer)
                {
                    UpdateControlledPlayerWithServerState(gameSnapshot);
                }
                else if (gameSnapshot.playersInfo.ContainsKey(clientPlayer.basePlayer.information.id))
                {
                    UpdateNotControlledPlayerWithServerState(gameSnapshot.playersInfo[clientPlayer.basePlayer.information.id]);
                }
            }
        }

        private void RemoveDisconnectedPlayers(ClientGameSnapshot gameSnapshot)
        {
            foreach (int playerId in playersManager.players.Keys.ToList())
            {
                if (gameSnapshot.playersInfo.ContainsKey(playerId))
                {
                    continue;
                }

                playersManager.RemovePlayer(playerId);
            }
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

        public void UpdateControlledPlayerWithServerState(ClientGameSnapshot gameSnapshot)
        {
            ClientControllablePlayer controlledClientPlayer = playersManager.controlledClientPlayer;
            controlledClientPlayer.clientControllable.RemoveObsoleteSnapshotStates(gameSnapshot);

            if (IsReconciliationNeeded(controlledClientPlayer, gameSnapshot))
            {
                Reconcile(controlledClientPlayer, gameSnapshot);
            }

            UpdatePlayerWithServerState(gameSnapshot.playersInfo[controlledClientPlayer.basePlayer.information.id]);
        }

        public void UpdateNotControlledPlayerWithServerState(SnapshotPlayerInfo snapshotPlayerInfo)
        {
            if (snapshotPlayerInfo.unseen && playersManager.players[snapshotPlayerInfo.id].gameObject.activeSelf)
            {
                playersManager.players[snapshotPlayerInfo.id].gameObject.SetActive(false);
                return;
            }
            else if (!snapshotPlayerInfo.unseen && !playersManager.players[snapshotPlayerInfo.id].gameObject.activeSelf)
            {
                // Todo: implement some kind of client anti-cheat (It is impossible to have a clear solution, check out my reddit question about it)
                // @link https://www.reddit.com/r/gamedev/comments/lcq93c/how_to_use_clientside_prediction_with_fog_of_war/
                playersManager.players[snapshotPlayerInfo.id].gameObject.SetActive(true);
            }

            // snapshotPlayerInfo.id, snapshotPlayerInfo.position, snapshotPlayerInfo.input, snapshotPlayerInfo.color, snapshotPlayerInfo.isImpostor
            ClientPlayer clientPlayer = playersManager.players[snapshotPlayerInfo.id];

            clientPlayer.basePlayer.controllable.playerInput = snapshotPlayerInfo.input;
            clientPlayer.basePlayer.movable.Teleport(snapshotPlayerInfo.position);

            UpdatePlayerWithServerState(snapshotPlayerInfo);
        }

        private void UpdatePlayerWithServerState(SnapshotPlayerInfo snapshotPlayerInfo)
        {
            ClientPlayer clientPlayer = playersManager.players[snapshotPlayerInfo.id];

            if (clientPlayer.basePlayer.colorable.color != snapshotPlayerInfo.color)
            {
                clientPlayer.basePlayer.colorable.ChangeColor(snapshotPlayerInfo.color);
            }

            clientPlayer.basePlayer.impostorable.isImpostor = snapshotPlayerInfo.isImpostor;
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
    }
}
