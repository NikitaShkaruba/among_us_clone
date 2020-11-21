using System;
using System.Collections.Generic;
using UnityEngine;
using Vector2 = AmongUsClone.Shared.DataStructures.Vector2;

namespace AmongUsClone.Client
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private GameObject playersParentGameObject;
        [SerializeField] public GameObject localPlayerPrefab;
        [SerializeField] public GameObject remotePlayerPrefab;

        private readonly Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

        public void AddPlayer(int playerId, string playerName, Vector2 playerPosition)
        {
            GameObject playerPrefab = playerId == Networking.Client.instance.id ? localPlayerPrefab : remotePlayerPrefab;
            GameObject player = Instantiate(playerPrefab, new Vector3(playerPosition.x, playerPosition.y, 0), Quaternion.identity);
            player.transform.parent = playersParentGameObject.transform;

            Debug.Log($"player position: {playerPosition.x}, {playerPosition.y}");

            PlayerManager playerManager = player.GetComponent<PlayerManager>();

            playerManager.id = playerId;
            playerManager.name = playerName;

            players.Add(playerId, playerManager);
        }

        public void RemovePlayer(int playerId)
        {
            if (!players.ContainsKey(playerId))
            {
                throw new Exception("Unable to remove non existent player");
            }

            Destroy(players[playerId].gameObject);
            players.Remove(playerId);
        }

        public void UpdatePlayerPosition(int playerId, Vector2 playerPosition)
        {
            // Because of multi threading, we might not have a player yet 
            if (!players.ContainsKey(playerId))
            {
                return;
            }

            Debug.Log($"Update player position: {playerPosition.x}, {playerPosition.y}");

            players[playerId].transform.position = new Vector3(playerPosition.x, playerPosition.y, 0);
        }

        public void Reset()
        {
            foreach (PlayerManager player in players.Values)
            {
                Destroy(player.gameObject);
            }
            
            players.Clear();
        }
    }
}
