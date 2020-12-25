// ReSharper disable UnusedMember.Global

using System;
using System.Linq;
using UnityEngine;

namespace AmongUsClone.Shared.Logging
{
    public static class Logger
    {
        private static readonly DateTime startupDateTime = DateTime.Now;

        private const string LogTypeEvent = "event";
        private const string LogTypeNotice = "notice";
        private const string LogTypeError = "error";
        private const string LogTypeCritical = "critical";

        public static void LogDebug(string eventDescription)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Log(LogTypeEvent, LoggerSection.Debug, eventDescription);
        }

        public static void LogEvent(LoggerSection loggerSection, string eventDescription)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Log(LogTypeEvent, loggerSection, eventDescription);
        }

        public static void LogNotice(LoggerSection loggerSection, string errorDescription)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Log(LogTypeNotice, loggerSection, errorDescription);
        }

        public static void LogError(LoggerSection loggerSection, string errorDescription)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log(LogTypeError, loggerSection, errorDescription);
        }

        // Isn't used on client yet
        // ReSharper disable once UnusedMember.Global
        public static void LogCritical(LoggerSection loggerSection, string criticalDescription)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Log(LogTypeCritical, loggerSection, criticalDescription);
        }

        private static void Log(string logType, LoggerSection loggerSection, string logDescription)
        {
            if (logType == LogTypeEvent && IsSkippedLoggerSection(loggerSection))
            {
                return;
            }
            if (logType == LogTypeNotice && AreNoticesSkipped())
            {
                return;
            }

            Debug.Log($"{loggerSection} {logType} - {logDescription}");
        }

        private static bool IsSkippedLoggerSection(LoggerSection loggerSection)
        {
            LoggerSection[] skippedLoggerSections = {
                LoggerSection.Network,
                LoggerSection.GameSnapshots,
            };

            return skippedLoggerSections.Contains(loggerSection);
        }

        private static bool AreNoticesSkipped()
        {
            return true;
        }
    }
}
