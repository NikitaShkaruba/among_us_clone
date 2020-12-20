using System.Collections.Generic;
using AmongUsClone.Shared.Logging;

namespace AmongUsClone.Server.Snapshots
{
    public static class LastClientRequestIds
    {
        public static readonly Dictionary<int, int> lastPlayerRequestIds = new Dictionary<int, int>();

        public static void Initialize(int playerId)
        {
            lastPlayerRequestIds[playerId] = 0;
        }

        public static void Update(int playerId, int lastRequestId)
        {
            if (!lastPlayerRequestIds.ContainsKey(playerId))
            {
                Initialize(playerId);
            }

            if (lastPlayerRequestIds[playerId] > lastRequestId)
            {
                Logger.LogError(LoggerSection.GameSnapshots, $"Got less or equal lastClientControlRequestId. Last remembered: {lastPlayerRequestIds[playerId]}, new: {lastRequestId}");
                return;
            }

            lastPlayerRequestIds[playerId] = lastRequestId;
        }

        public static int Get(int playerId)
        {
            if (!lastPlayerRequestIds.ContainsKey(playerId))
            {
                Initialize(playerId);
            }

            return lastPlayerRequestIds[playerId];
        }
    }
}
