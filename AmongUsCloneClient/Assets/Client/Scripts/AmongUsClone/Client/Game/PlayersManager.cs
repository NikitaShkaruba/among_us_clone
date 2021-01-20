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
        public readonly Dictionary<int, Player> players = new Dictionary<int, Player>();
        public Player controlledPlayer;

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
            players[playerId].controllable.playerInput = playerInput;
            players[playerId].movable.Teleport(playerPosition);
        }

        public void AddPlayer(int playerId, Player player)
        {
            players[playerId] = player;
            playersAmountChanged?.Invoke();
        }

        public void RemovePlayer(int playerId)
        {
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
