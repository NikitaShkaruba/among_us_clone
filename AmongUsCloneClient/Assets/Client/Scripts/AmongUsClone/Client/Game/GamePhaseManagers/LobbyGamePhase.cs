using System;
using System.Collections.Generic;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Networking;
using AmongUsClone.Client.UI;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    [CreateAssetMenu(fileName = "LobbyGamePhase", menuName = "LobbyGamePhase")]
    public class LobbyGamePhase : ScriptableObject
    {
        public Lobby.Lobby lobby;
        public Dictionary<int, Player> players = new Dictionary<int, Player>();
        public Player controlledPlayer;

        [SerializeField] public GameObject clientControllablePlayerPrefab;
        [SerializeField] public GameObject playerPrefab;

        public ConnectionToServer connectionToServer;

        public Action playersAmountChanged;

        public int maxPlayersAmount;
        public int minRequiredPlayersAmountForGame;
        public int secondsForGameLaunch;

        // Todo: remove
        public UserInterface userInterface;

        public void InitializeGameSettings(int maxPlayersAmount, int minRequiredPlayersAmountForGame, int secondsForGameLaunch)
        {
            this.maxPlayersAmount = maxPlayersAmount;
            this.minRequiredPlayersAmountForGame = minRequiredPlayersAmountForGame;
            this.secondsForGameLaunch = secondsForGameLaunch;
        }

        public void DisconnectFromLobby()
        {
            throw new NotImplementedException();
            // userInterface.RemovePauseMenu();
            // lobby.gameObject.SetActive(false);
            // gameRoleDescription.gameObject.SetActive(false);
            // mainMenu.gameObject.SetActive(true);
            // mainMenu.Reset();

            // foreach (Player player in players.Values)
            // {
                // Destroy(player.gameObject);
            // }

            // players.Clear();
            // playersAmountChanged?.Invoke();

            // connectionToServer.Disconnect();
        }

        public void RemovePlayerFromLobby(int playerId)
        {
            if (!players.ContainsKey(playerId))
            {
                throw new Exception("Unable to remove non existent player");
            }

            Destroy(players[playerId].gameObject);
            players.Remove(playerId);
            playersAmountChanged?.Invoke();
        }

        private void InitializeControlledPlayer(Player player)
        {
            controlledPlayer = player;

            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();
            playerCamera.target = controlledPlayer.gameObject;
            playerCamera.transform.position = Vector3.zero;

            lobby.interactButton.SetInteractor(player.interactor);

            if (controlledPlayer.information.isLobbyHost)
            {
                lobby.gameStartable.ShowStartButtonForHost();
            }
        }

        public bool HasEnoughPlayersForGame()
        {
            return players.Count >= minRequiredPlayersAmountForGame;
        }

        public void StartGame(bool isPlayingImpostor, int impostorsAmount, int[] impostorPlayerIds)
        {
            // ScenesManager.LoadScene(Scenes.RoleShowcase);
            throw new NotImplementedException();

            // this.impostorPlayerIds = impostorPlayerIds;
            // this.impostorsAmount = impostorsAmount;

            // if (isPlayingImpostor)
            // {
            // foreach (int impostorPlayerId in impostorPlayerIds)
            // {
            // players[impostorPlayerId].information.isImposter = true;
            // }
            // }

            // lobby.gameObject.SetActive(false);
            // mainMenu.gameObject.SetActive(false);
            // gameRoleDescription.gameObject.SetActive(true);

            // Logger.LogDebug($"Game has started. Impostors: {this.impostorsAmount}");
        }

        public void AddPlayerToLobby(int playerId, string playerName, PlayerColor playerColor, Vector2 playerPosition, bool isPlayerHost)
        {
            throw new NotImplementedException();
            // bool isControlledPlayerConnecting = playerId == connectionToServer.myPlayerId;

            // if (mainMenu.gameObject.activeSelf && isControlledPlayerConnecting)
            // {
                // mainMenu.gameObject.SetActive(false);
                // lobby.gameObject.SetActive(true);
            // }

            // GameObject chosenPlayerPrefab = isControlledPlayerConnecting ? clientControllablePlayerPrefab : playerPrefab;
            // players[playerId] = lobby.playersContainable.AddPlayer(playerPosition, chosenPlayerPrefab).GetComponent<Player>();
            // players[playerId].Initialize(playerId, playerName, playerColor, isPlayerHost);
            // playersAmountChanged?.Invoke();

            // if (isControlledPlayerConnecting)
            // {
                // InitializeControlledPlayer(players[playerId]);
            // }
        }


        public void UpdatePlayerPosition(int playerId, Vector2 playerPosition)
        {
            players[playerId].movable.Teleport(playerPosition);
        }

        public void UpdatePlayerWithServerState(int playerId, Vector2 playerPosition, PlayerInput playerInput)
        {
            players[playerId].controllable.playerInput = playerInput;
            UpdatePlayerPosition(playerId, playerPosition);
        }

        public void ChangePlayerColor(int playerId, PlayerColor playerColor)
        {
            players[playerId].colorable.ChangeColor(playerColor);
            Logger.LogEvent(SharedLoggerSection.PlayerColors, $"Changed player {playerId} color to {Shared.Helpers.GetEnumName(playerColor)}");
        }
    }
}
