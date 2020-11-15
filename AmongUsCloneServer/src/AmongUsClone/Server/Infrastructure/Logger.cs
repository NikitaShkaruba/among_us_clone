using System;
using System.Linq;

namespace AmongUsClone.Server.Infrastructure
{
    public static class Logger
    {
        private const string LogTypeEvent = "event";
        private const string LogTypeError = "error";

        public static void LogEvent(LoggerSection loggerSection, string eventDescription)
        {
            Log(LogTypeEvent, loggerSection, eventDescription);
        }

        public static void LogError(LoggerSection loggerSection, string errorDescription)
        {
            Log(LogTypeError, loggerSection, errorDescription);
        }

        private static void Log(string logType, LoggerSection loggerSection, string logDescription)
        {
            if (IsSkippedLoggerSection(loggerSection) && logType != LogTypeError)
            {
                return;
            }
            
            Console.WriteLine($"[{loggerSection} {logType}] {logDescription}");
        }

        private static bool IsSkippedLoggerSection(LoggerSection loggerSection)
        {
            LoggerSection[] skippedLoggerSections = {
                LoggerSection.Network,
            };

            return skippedLoggerSections.Contains(loggerSection);
        }
    }
}
