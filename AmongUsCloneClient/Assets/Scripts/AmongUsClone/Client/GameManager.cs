using AmongUsClone.Client.UI;
using UnityEngine;
using Vector2 = AmongUsClone.Shared.DataStructures.Vector2;

namespace AmongUsClone.Client
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public UserInterface userInterface;
        public GameObject mainMenu;
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

        // Unity holds some data between running game instances, so we need to cleanup by hand
        private void OnApplicationQuit()
        {
            Networking.Client.instance.DisconnectFromServer();
        }

        public void ConnectToLobby()
        {
            instance.mainMenu.SetActive(false);
            instance.lobby.gameObject.SetActive(true);
            
            Networking.Client.instance.ConnectToServer();
        }

        public void DisconnectFromLobby()
        {
            userInterface.RemovePauseMenu();
            instance.mainMenu.SetActive(true);
            instance.lobby.gameObject.SetActive(false);
            lobby.Reset();
            
            Networking.Client.instance.DisconnectFromServer();
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
