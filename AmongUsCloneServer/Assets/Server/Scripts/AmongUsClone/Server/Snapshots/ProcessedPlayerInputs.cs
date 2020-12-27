using System.Collections.Generic;
using AmongUsClone.Shared.Logging;

namespace AmongUsClone.Server.Snapshots
{
    public static class ProcessedPlayerInputs
    {
        public static readonly Dictionary<int, int> lastPlayersProcessedInputIds = new Dictionary<int, int>();

        public static void Initialize(int playerId)
        {
            lastPlayersProcessedInputIds[playerId] = 0;
        }

        public static void Update(int playerId, int lastProcessedInputId)
        {
            if (!lastPlayersProcessedInputIds.ContainsKey(playerId))
            {
                Initialize(playerId);
            }

            if (lastPlayersProcessedInputIds[playerId] > lastProcessedInputId)
            {
                Logger.LogError(LoggerSection.GameSnapshots, $"Got less or equal last inputId. Last remembered: {lastPlayersProcessedInputIds[playerId]}, new: {lastProcessedInputId}");
                return;
            }

            lastPlayersProcessedInputIds[playerId] = lastProcessedInputId;
        }

        public static int Get(int playerId)
        {
            if (!lastPlayersProcessedInputIds.ContainsKey(playerId))
            {
                Initialize(playerId);
            }

            return lastPlayersProcessedInputIds[playerId];
        }
    }
}
