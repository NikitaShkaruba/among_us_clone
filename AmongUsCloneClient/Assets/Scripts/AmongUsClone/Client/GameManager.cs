using System.Collections.Generic;
using UnityEngine;
using Vector2 = AmongUsClone.Shared.DataStructures.Vector2;

namespace AmongUsClone.Client
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public static readonly Dictionary<int, PlayerManager> Players = new Dictionary<int, PlayerManager>();
        
        public GameObject localPlayerPrefab;
        public GameObject remotePlayerPrefab;

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

        public void SpawnPlayer(int id, string name, Vector2 position)
        {
            GameObject playerPrefab = id == Networking.Client.instance.id ? localPlayerPrefab : remotePlayerPrefab;
            GameObject player = Instantiate(playerPrefab, new Vector3(position.x, position.y, 0), Quaternion.identity);
            
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            
            playerManager.id = id;
            playerManager.name = name;
            
            Players.Add(id, playerManager);
        }
    }
}
