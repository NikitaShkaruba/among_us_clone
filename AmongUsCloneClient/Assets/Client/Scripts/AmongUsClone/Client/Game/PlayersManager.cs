using System;
using System.Collections.Generic;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Client.Game
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PlayersManager", menuName = "PlayersManager")]
    public class PlayersManager : ScriptableObject
    {
        public readonly Dictionary<int, ClientPlayer> players = new Dictionary<int, ClientPlayer>();
        public ClientControllablePlayer controlledClientPlayer;

        public int[] knownImpostorPlayerIds;
        public int impostorsAmount;

        public Action playersAmountChanged;

        public void InitializeImpostorsData(int impostorsAmount, int[] knownImpostorPlayerIds)
        {
            this.impostorsAmount = impostorsAmount;
            this.knownImpostorPlayerIds = knownImpostorPlayerIds;
        }

        public void UpdatePlayerWithServerState(int playerId, Vector2 playerPosition, PlayerInput playerInput)
        {
            players[playerId].basePlayer.controllable.playerInput = playerInput;
            players[playerId].basePlayer.movable.Teleport(playerPosition);
        }

        public void AddPlayer(int playerId, ClientPlayer clientPlayer)
        {
            players[playerId] = clientPlayer;
            playersAmountChanged?.Invoke();
        }

        public void RemovePlayer(int playerId)
        {
            if (!players.ContainsKey(playerId))
            {
                throw new Exception("Unable to remove non existent player");
            }

            Destroy(players[playerId].gameObject);
            players.Remove(playerId);
            playersAmountChanged?.Invoke();
        }

        public void ClearPlayers()
        {
            players.Clear();
            playersAmountChanged?.Invoke();
        }
    }
}
