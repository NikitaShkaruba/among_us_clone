using System;
using System.Collections.Generic;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private GameObject playersParentGameObject;
        [SerializeField] public GameObject clientControllablePlayerPrefab;
        [SerializeField] public GameObject playerPrefab;

        private readonly Dictionary<int, Player> players = new Dictionary<int, Player>();

        public void AddPlayer(int playerId, string playerName, Vector2 playerPosition)
        {
            GameObject chosenPlayerPrefab = playerId == Game.instance.connectionToServer.myPlayerId ? clientControllablePlayerPrefab : playerPrefab;
            GameObject playerGameObject = Instantiate(chosenPlayerPrefab, new Vector3(playerPosition.x, playerPosition.y, 0), Quaternion.identity);
            playerGameObject.transform.parent = playersParentGameObject.transform;

            Player player = playerGameObject.GetComponent<Player>();
            player.Initialize(playerId, playerName);

            players.Add(playerId, player);
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

            players[playerId].movable.Move(playerPosition);
        }

        public void Reset()
        {
            foreach (Player player in players.Values)
            {
                Destroy(player.gameObject);
            }

            players.Clear();
        }
    }
}
