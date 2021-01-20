using System;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Networking;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Game;
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
        [SerializeField] private GameObject clientControllablePlayerPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private ConnectionToServer connectionToServer;
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PacketsSender packetsSender;

        public Lobby.Lobby lobby;

        public int maxPlayersAmount;
        public int minRequiredPlayersAmountForGame;
        public int secondsForGameLaunch;

        public void Initialize()
        {
            lobby = FindObjectOfType<Lobby.Lobby>();
            lobby.gameObject.SetActive(false); // Make it hidden before a first player joins
        }

        public void InitializeGameSettings(int maxPlayersAmount, int minRequiredPlayersAmountForGame, int secondsForGameLaunch)
        {
            this.maxPlayersAmount = maxPlayersAmount;
            this.minRequiredPlayersAmountForGame = minRequiredPlayersAmountForGame;
            this.secondsForGameLaunch = secondsForGameLaunch;
        }

        public void AddPlayerToLobby(int playerId, string playerName, PlayerColor playerColor, Vector2 playerPosition, bool isPlayerHost)
        {
            bool isControlledPlayerConnecting = playerId == connectionToServer.myPlayerId;

            if (lobby.gameObject.activeSelf == false)
            {
                ScenesManager.UnloadScene(Scene.MainMenu);
                lobby.gameObject.SetActive(true);
            }

            GameObject chosenPlayerPrefab = isControlledPlayerConnecting ? clientControllablePlayerPrefab : playerPrefab;
            Player player = Instantiate(chosenPlayerPrefab, playerPosition, Quaternion.identity).GetComponent<Player>();
            player.Initialize(playerId, playerName, playerColor, isPlayerHost);
            lobby.playersContainer.PlacePlayerIntoPlayersContainer(player.gameObject);
            playersManager.AddPlayer(playerId, player);

            if (isControlledPlayerConnecting)
            {
                InitializeControlledPlayer(playersManager.players[playerId]);
            }
        }

        public void RemovePlayerFromLobby(int playerId)
        {
            if (!playersManager.players.ContainsKey(playerId))
            {
                throw new Exception("Unable to remove non existent player");
            }

            Destroy(playersManager.players[playerId].gameObject);
            playersManager.RemovePlayer(playerId);
        }

        public void ChangePlayerColor(int playerId, PlayerColor playerColor)
        {
            playersManager.players[playerId].colorable.ChangeColor(playerColor);
            Logger.LogEvent(SharedLoggerSection.PlayerColors, $"Changed player {playerId} color to {Shared.Helpers.GetEnumName(playerColor)}");
        }

        public void RequestGameStart()
        {
            if (!HasEnoughPlayersForGame())
            {
                return;
            }

            packetsSender.SendStartGamePacket();
        }

        public void InitiateGameStart()
        {
            lobby.gameStartable.InitiateGameStart();
        }

        public void StartGame(bool isPlayingImpostor, int impostorsAmount, int[] impostorPlayerIds)
        {
            playersManager.InitializeImpostorsData(impostorsAmount, impostorPlayerIds);

            if (isPlayingImpostor)
            {
                foreach (int impostorPlayerId in impostorPlayerIds)
                {
                    playersManager.players[impostorPlayerId].information.isImposter = true;
                }
            }

            ScenesManager.SwitchScene(Scene.RoleReveal);

            Logger.LogDebug($"Game has started. Impostors amount: {impostorsAmount}");
        }

        private void InitializeControlledPlayer(Player player)
        {
            playersManager.controlledPlayer = player;

            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();
            playerCamera.target = playersManager.controlledPlayer.gameObject;
            playerCamera.transform.position = Vector3.zero;

            lobby.interactButton.SetInteractor(player.interactor);

            if (playersManager.controlledPlayer.information.isLobbyHost)
            {
                lobby.gameStartable.ShowStartButtonForHost();
            }
        }

        public bool HasEnoughPlayersForGame()
        {
            return playersManager.players.Count >= minRequiredPlayersAmountForGame;
        }
    }
}
