// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Shared.Game
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private GameObject playersParentGameObject;

        private readonly Dictionary<int, Player> players = new Dictionary<int, Player>();

        public Player AddPlayer(int playerId, string playerName, Vector2 playerPosition, GameObject playerPrefab)
        {
            Player player = Instantiate(playerPrefab, new Vector3(playerPosition.x, playerPosition.y, 0), Quaternion.identity).GetComponent<Player>();
            player.Initialize(playerId, playerName);
            player.transform.parent = playersParentGameObject.transform;

            players.Add(playerId, player);

            return player;
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
