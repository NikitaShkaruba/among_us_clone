using System.Collections.Generic;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Shared.Game.Interactions
{
    public class PlayersLockable : MonoBehaviour
    {
        [SerializeField] private BasePlayersManager basePlayersManager;

        private readonly List<int> lockedPlayerIds = new List<int>();

        public void Add(int playerId)
        {
            lockedPlayerIds.Add(playerId);

            BasePlayer basePlayer = basePlayersManager.players[playerId];
            if (basePlayer.movable.isDisabled)
            {
                Logger.LogError(SharedLoggerSection.Interactions, $"Unable to lock the disabled player {playerId}");
                return;
            }

            basePlayer.movable.isDisabled = true;
        }

        public void Remove(int playerId)
        {
            lockedPlayerIds.Remove(playerId);

            BasePlayer basePlayer = basePlayersManager.players[playerId];
            if (!basePlayer.movable.isDisabled)
            {
                Logger.LogError(SharedLoggerSection.Interactions, $"Unable to unlock the not disabled player {playerId}");
                return;
            }

            basePlayer.movable.isDisabled = false;
        }

        public bool IsPlayerLocked(int playerId)
        {
            return lockedPlayerIds.Contains(playerId);
        }
    }
}
