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
    // [CreateAssetMenu(fileName = "LobbyGamePhase", menuName = "LobbyGamePhase")]
    public class LobbyGamePhase : ScriptableObject
    {
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private ConnectionToServer connectionToServer;

        [SerializeField] private GameObject clientControllablePlayerPrefab;
        [SerializeField] private GameObject playerPrefab;

        public Lobby.Lobby lobby;

        public const int MaxPlayersAmount = GameConfiguration.MaxPlayersAmount;
        public const int MinRequiredPlayersAmountForGame = GameConfiguration.MinRequiredPlayersAmountForGame;
        public const int SecondsForGameLaunch = GameConfiguration.SecondsForGameLaunch;

        public void Initialize()
        {
            lobby = FindObjectOfType<Lobby.Lobby>();
        }

        public void AddPlayerToLobby(int playerId, string playerName, PlayerColor playerColor, Vector2 playerPosition, bool playerLookingRight, bool isPlayerHost)
        {
            bool isControlledPlayerConnecting = playerId == connectionToServer.myPlayerId;

            GameObject chosenPlayerPrefab = isControlledPlayerConnecting ? clientControllablePlayerPrefab : playerPrefab;
            ClientPlayer clientPlayer = Instantiate(chosenPlayerPrefab, playerPosition, Quaternion.identity).GetComponent<ClientPlayer>();
            clientPlayer.name = isControlledPlayerConnecting ? $"Player{playerId} (ClientControllable)" : $"Player{playerId}";
            clientPlayer.Initialize(playerId, playerName, playerColor, isPlayerHost);
            clientPlayer.transform.parent = lobby.playersContainer.transform;
            clientPlayer.animator.isLookingRight = playerLookingRight;

            playersManager.AddPlayer(playerId, clientPlayer);

            if (isControlledPlayerConnecting)
            {
                InitializeControlledPlayer(playersManager.players[playerId].GetComponent<ClientControllablePlayer>());
            }
        }

        public void ChangePlayerColor(int playerId, PlayerColor playerColor)
        {
            playersManager.players[playerId].basePlayer.colorable.ChangeColor(playerColor);
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
                    playersManager.players[impostorPlayerId].basePlayer.impostorable.isImpostor = true;
                }
            }

            scenesManager.LoadScene(Scene.RoleReveal);
        }

        private void InitializeControlledPlayer(ClientControllablePlayer clientControllablePlayer)
        {
            playersManager.controlledClientPlayer = clientControllablePlayer;

            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();
            playerCamera.target = playersManager.controlledClientPlayer.gameObject;
            playerCamera.transform.position = Vector3.zero;

            lobby.interactButton.SetInteractor(clientControllablePlayer.interactor);

            if (playersManager.controlledClientPlayer.basePlayer.gameHostable.isHost)
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
