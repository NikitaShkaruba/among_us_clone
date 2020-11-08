using System;

namespace AmongUsClone.Server.Infrastructure
{
    public static class Logger
    {
        public static void LogEvent(string eventDescription)
        {
            Log("Event", eventDescription);
        }

        public static void LogError(string errorDescription)
        {
            Log("Error", errorDescription);
        }

        private static void Log(string logType, string logDescription)
        {
            Console.WriteLine($"[{logType}] {logDescription}");
        }
    }
}
