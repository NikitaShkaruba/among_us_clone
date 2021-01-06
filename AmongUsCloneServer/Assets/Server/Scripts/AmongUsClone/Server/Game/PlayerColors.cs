using System;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;

namespace AmongUsClone.Server.Game
{
    public static class PlayerColors
    {
        static Dictionary<PlayerColor, int> occupiedColors = new Dictionary<PlayerColor, int>();

        public static PlayerColor TakeFreeColor(int playerId)
        {
            PlayerColor color = GetFreeColor();
            TakeColor(color, playerId);

            return color;
        }

        public static PlayerColor SwitchToRandomColor(int playerId)
        {
            if (!occupiedColors.ContainsValue(playerId))
            {
                throw new Exception("Unable to switch color if not having one");
            }

            PlayerColor color = GetFreeColor();
            ReleasePlayerColor(playerId);
            TakeColor(color, playerId);

            return color;
        }

        public static void ReleasePlayerColor(int playerId)
        {
            if (!occupiedColors.ContainsValue(playerId))
            {
                throw new Exception("Attempt to release not occupied color");
            }

            PlayerColor playerColor = occupiedColors.FirstOrDefault(x => x.Value == playerId).Key;
            occupiedColors.Remove(playerColor);

            Logger.LogEvent(LoggerSection.PlayerColors, $"Released {Helpers.GetEnumName(playerColor)} color from player {playerId}");
        }

        private static void TakeColor(PlayerColor color, int playerId)
        {
            if (occupiedColors.ContainsValue(playerId))
            {
                throw new Exception("Attempt to take already occupied color");
            }

            occupiedColors[color] = playerId;
            Logger.LogEvent(LoggerSection.PlayerColors, $"Took {Helpers.GetEnumName(color)} color for player {playerId}");
        }

        private static PlayerColor GetFreeColor()
        {
            IEnumerable<PlayerColor> availablePlayerColors = GetShuffledAvailablePlayerColors();

            foreach (PlayerColor color in availablePlayerColors)
            {
                if (occupiedColors.ContainsKey(color))
                {
                    continue;
                }

                return color;
            }

            throw new Exception("Attempt to take already occupied color");
        }

        private static IEnumerable<PlayerColor> GetShuffledAvailablePlayerColors()
        {
            IEnumerable<PlayerColor> availablePlayerColors = Enum.GetValues(typeof(PlayerColor)).Cast<PlayerColor>();

            Random random = new Random();
            IEnumerable<PlayerColor> shuffledList = availablePlayerColors
                .Select(x => new {Number = random.Next(), Item = x})
                .OrderBy(x => x.Number)
                .Select(x => x.Item);

            return shuffledList.ToList();
        }
    }
}
