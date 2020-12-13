using AmongUsClone.Client.Networking;
using AmongUsClone.Client.UI;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client
{
    public class Game : MonoBehaviour
    {
        public static Game instance;

        public readonly ConnectionToServer connectionToServer = new ConnectionToServer();

        public UserInterface userInterface;
        public MainMenu mainMenu;
        public Lobby lobby;

        [SerializeField] public GameObject clientControllablePlayerPrefab;
        [SerializeField] public GameObject playerPrefab;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Logger.LogError(LoggerSection.Initialization, "Instance already exists, destroying the object!");
                Destroy(this);
            }
        }

        private void Update()
        {
            MainThread.Execute();
        }

        // Unity holds some data between running game instances, so we need to cleanup by hand
        private void OnApplicationQuit()
        {
            connectionToServer.Disconnect();
        }

        public void ConnectToLobby()
        {
            mainMenu.gameObject.SetActive(false);
            lobby.gameObject.SetActive(true);

            connectionToServer.Connect();
        }

        public void DisconnectFromLobby()
        {
            userInterface.RemovePauseMenu();
            mainMenu.gameObject.SetActive(true);
            lobby.gameObject.SetActive(false);
            lobby.Reset();

            connectionToServer.Disconnect();
        }

        public void AddPlayerToLobby(int playerId, string playerName, Vector2 playerPosition)
        {
            GameObject chosenPlayerPrefab = playerId == Game.instance.connectionToServer.myPlayerId ? clientControllablePlayerPrefab : playerPrefab;
            lobby.AddPlayer(playerId, playerName, playerPosition, chosenPlayerPrefab);
        }

        public void RemovePlayerFromLobby(int playerId)
        {
            lobby.RemovePlayer(playerId);
        }

        public void UpdatePlayerPosition(int playerId, Vector2 playerPosition)
        {
            lobby.UpdatePlayerPosition(playerId, playerPosition);
        }
    }
}
