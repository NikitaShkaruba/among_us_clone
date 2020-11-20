using System;
using System.Collections.Generic;
using System.Linq;
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

        public void SpawnPlayer(int playerId, string playerName, Vector2 playerPosition)
        {
            GameObject playerPrefab = playerId == Networking.Client.instance.id ? localPlayerPrefab : remotePlayerPrefab;
            GameObject player = Instantiate(playerPrefab, new Vector3(playerPosition.x, playerPosition.y, 0), Quaternion.identity);
            
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            
            playerManager.id = playerId;
            playerManager.name = playerName;
            
            Players.Add(playerId, playerManager);
        }

        public static void RemovePlayer(int playerId)
        {
            PlayerManager[] gameManagers = FindObjectsOfType<PlayerManager>();
            PlayerManager manager = gameManagers.Single(playerManager => playerManager.id == playerId);
            if (manager == null)
            {
                throw new Exception("Unable to remove disconnected player");
            }
            
            Destroy(manager.gameObject);
            Players.Remove(playerId);
        }
    }
}
