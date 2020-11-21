using UnityEngine;
using Vector2 = AmongUsClone.Shared.DataStructures.Vector2;

namespace AmongUsClone.Client
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

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

        public void ConnectToLobby()
        {
            instance.mainMenu.SetActive(false);
            instance.lobby.gameObject.SetActive(true);
            
            Networking.Client.instance.ConnectToServer();
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
