using System;
using System.Linq;

namespace AmongUsClone.Server.Infrastructure
{
    public static class Logger
    {
        private static readonly DateTime startupDateTime = DateTime.Now;

        private const string LogTypeEvent = "event";
        private const string LogTypeError = "error";

        public static void LogEvent(LoggerSection loggerSection, string eventDescription)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Log(LogTypeEvent, loggerSection, eventDescription);
        }

        public static void LogError(LoggerSection loggerSection, string errorDescription)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log(LogTypeError, loggerSection, errorDescription);
        }

        private static void Log(string logType, LoggerSection loggerSection, string logDescription)
        {
            if (IsSkippedLoggerSection(loggerSection) && logType != LogTypeError)
            {
                return;
            }

            Console.WriteLine($"[{RenderTimeSinceStartupLabel()} {loggerSection} {logType}] {logDescription}");
        }

        private static bool IsSkippedLoggerSection(LoggerSection loggerSection)
        {
            LoggerSection[] skippedLoggerSections = {
                LoggerSection.Network,
            };

            return skippedLoggerSections.Contains(loggerSection);
        }

        private static string RenderTimeSinceStartupLabel()
        {
            float secondsSinceStart = (DateTime.Now - startupDateTime).Seconds;

            const int minutesInHour = 60;
            const int secondsInMinute = 60;
            const int secondsInHour = 60 * secondsInMinute;

            int hoursToDisplay = (int)Math.Floor(secondsSinceStart / secondsInHour);
            int minutesToDisplay = (int)Math.Floor(secondsSinceStart / secondsInMinute % minutesInHour);
            int secondsToDisplay = (int)Math.Floor(secondsSinceStart - Math.Floor(secondsSinceStart / secondsInMinute) * secondsInMinute);

            string timeLabel = $"{minutesToDisplay:D2}:{secondsToDisplay:D2}";
            if (hoursToDisplay > 0)
            {
                timeLabel = $"{hoursToDisplay:D2}:{timeLabel}";
            }

            return timeLabel;
        }
    }
}
