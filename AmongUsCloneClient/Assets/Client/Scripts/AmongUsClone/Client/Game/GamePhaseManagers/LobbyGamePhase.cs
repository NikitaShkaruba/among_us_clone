using System;
using System.Collections.Generic;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Networking;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "LobbyGamePhase", menuName = "LobbyGamePhase")]
    public class LobbyGamePhase : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private ConnectionToServer connectionToServer;

        [SerializeField] private GameObject clientControllablePlayerPrefab;
        [SerializeField] private GameObject playerPrefab;

        public Lobby.Lobby lobby;

        private List<Action> onSceneLoadedActions = new List<Action>();
        private bool sceneLoadRequested;

        public const int MaxPlayersAmount = GameConfiguration.MaxPlayersAmount;
        public const int MinRequiredPlayersAmountForGame = GameConfiguration.MinRequiredPlayersAmountForGame;
        public const int SecondsForGameLaunch = GameConfiguration.SecondsForGameLaunch;

        public void Reset()
        {
            sceneLoadRequested = false;
        }

        public void Initialize()
        {
            lobby = FindObjectOfType<Lobby.Lobby>();
        }

        public void AddPlayerToLobby(int playerId, string playerName, PlayerColor playerColor, Vector2 playerPosition, bool isPlayerHost)
        {
            bool isControlledPlayerConnecting = playerId == connectionToServer.myPlayerId;

            if (lobby == false)
            {
                InitializeLobby(playerId, playerName, playerColor, playerPosition, isPlayerHost);
                return;
            }

            GameObject chosenPlayerPrefab = isControlledPlayerConnecting ? clientControllablePlayerPrefab : playerPrefab;
            Player player = Instantiate(chosenPlayerPrefab, playerPosition, Quaternion.identity).GetComponent<Player>();
            player.Initialize(playerId, playerName, playerColor, isPlayerHost);
            player.transform.parent = lobby.playersContainer.transform;
            playersManager.AddPlayer(playerId, player);

            if (isControlledPlayerConnecting)
            {
                InitializeControlledPlayer(playersManager.players[playerId]);
            }
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

            scenesManager.LoadScene(Scene.RoleReveal);

            Logger.LogDebug($"Game has started. Impostors amount: {impostorsAmount}");
        }

        private void InitializeLobby(int playerId, string playerName, PlayerColor playerColor, Vector2 playerPosition, bool isPlayerHost)
        {
            // We cannot instantly load a scene and then add a player to it - this is made at the next frame.
            // In order to solve it, we switch a scene and pass a callback where all wanted players will be added
            onSceneLoadedActions.Add(() => AddPlayerToLobby(playerId, playerName, playerColor, playerPosition, isPlayerHost));

            if (!sceneLoadRequested)
            {
                scenesManager.SwitchScene(Scene.Lobby, OnLobbySceneLoaded);
                sceneLoadRequested = true;
            }
        }

        private void OnLobbySceneLoaded()
        {
            foreach (Action onSceneLoadAction in onSceneLoadedActions)
            {
                onSceneLoadAction();
            }

            sceneLoadRequested = false;
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
            return playersManager.players.Count >= MinRequiredPlayersAmountForGame;
        }
    }
}
