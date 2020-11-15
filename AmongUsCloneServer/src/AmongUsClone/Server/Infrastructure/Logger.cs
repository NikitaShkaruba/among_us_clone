using System;

namespace AmongUsClone.Server.Infrastructure
{
    public static class Logger
    {
        public static void LogEvent(LoggerSection loggerSection, string eventDescription)
        {
            Log("event", loggerSection, eventDescription);
        }

        public static void LogError(LoggerSection loggerSection, string errorDescription)
        {
            Log("error", loggerSection, errorDescription);
        }

        private static void Log(string logType, LoggerSection loggerSection, string logDescription)
        {
            Console.WriteLine($"[{loggerSection} {logType}] {logDescription}");
        }
    }
}
