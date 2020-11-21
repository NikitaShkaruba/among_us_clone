using AmongUsClone.Client.Networking;
using AmongUsClone.Client.UI;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared;
using UnityEngine;
using Vector2 = AmongUsClone.Shared.DataStructures.Vector2;

namespace AmongUsClone.Client
{
    public class Game : MonoBehaviour
    {
        public static Game instance;

        public readonly ConnectionToServer connectionToServer = new ConnectionToServer();

        public UserInterface userInterface;
        public MainMenu mainMenu;
        public Lobby lobby;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying the object!");
                Destroy(this);
            }
        }

        private void Update()
        {
            ThreadManager.UpdateMain();
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
            lobby.AddPlayer(playerId, playerName, playerPosition);
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
