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
            IEnumerable<PlayerColor> availablePlayerColors = Enum.GetValues(typeof(PlayerColor)).Cast<PlayerColor>();

            foreach (PlayerColor color in availablePlayerColors)
            {
                if (occupiedColors.ContainsKey(color))
                {
                    continue;
                }

                TakeColor(color, playerId);
                return color;
            }

            Logger.LogError(LoggerSection.PlayerColors, $"Unable to take a free color for player {playerId}");
            return PlayerColor.Red;
        }

        // Todo: support color switching
        public static void SwitchColor(PlayerColor wantedColor, int playerId)
        {
            if (!occupiedColors.ContainsValue(playerId))
            {
                Logger.LogError(LoggerSection.PlayerColors, "Not able to switch color if not have one");
                return;
            }

            PlayerColor playerColor = occupiedColors.FirstOrDefault(x => x.Value == playerId).Key;
            if (playerColor == wantedColor)
            {
                Logger.LogError(LoggerSection.PlayerColors, "Not able to switch color to the same one");
                return;
            }

            ReleasePlayerColor(playerId);
            TakeColor(wantedColor, playerId);
        }

        public static void ReleasePlayerColor(int playerId)
        {
            if (!occupiedColors.ContainsValue(playerId))
            {
                Logger.LogError(LoggerSection.PlayerColors, "Attempt to release not occupied color");
                return;
            }

            PlayerColor playerColor = occupiedColors.FirstOrDefault(x => x.Value == playerId).Key;
            occupiedColors.Remove(playerColor);

            Logger.LogEvent(LoggerSection.PlayerColors, $"Released {Helpers.GetEnumName(playerColor)} color from player {playerId}");
        }

        private static void TakeColor(PlayerColor color, int playerId)
        {
            if (occupiedColors.ContainsValue(playerId))
            {
                Logger.LogError(LoggerSection.PlayerColors, "Attempt to take already occupied color");
                return;
            }

            occupiedColors[color] = playerId;
            Logger.LogEvent(LoggerSection.PlayerColors, $"Took {Helpers.GetEnumName(color)} color for player {playerId}");
        }
    }
}
