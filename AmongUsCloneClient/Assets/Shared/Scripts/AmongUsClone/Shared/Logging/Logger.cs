// ReSharper disable UnusedMember.Global

using System;
using System.Linq;
using UnityEngine;

namespace AmongUsClone.Shared.Logging
{
    public static class Logger
    {
        private const string LogTypeEvent = "event";
        private const string LogTypeNotice = "notice";
        private const string LogTypeError = "error";
        private const string LogTypeCritical = "critical";

        private const string InternalLoggerSectionMeta = "Meta";
        private const string InternalLoggerSectionDebug = "Debug";

        private static string[] skippedEventSections = new string[0];
        private static bool areNoticesSkipped;

        public static void Initialize(string[] skippedEventSections, bool areNoticesSkipped)
        {
            Logger.skippedEventSections = skippedEventSections;
            Logger.areNoticesSkipped = areNoticesSkipped;

            Log(LogTypeEvent, InternalLoggerSectionMeta, $"Logger initialized. AreNoticesSkipped: {areNoticesSkipped}, SkippedEventSections: [ {string.Join(", ", skippedEventSections)} ]");
        }

        public static void LogDebug(string eventDescription)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Log(LogTypeEvent, InternalLoggerSectionDebug, eventDescription);
        }

        public static void LogEvent(string loggerSection, string eventDescription)
        {
            if (skippedEventSections.Contains(loggerSection))
            {
                return;
            }

            Console.ForegroundColor = ConsoleColor.Gray;
            Log(LogTypeEvent, loggerSection, eventDescription);
        }

        public static void LogNotice(string loggerSection, string errorDescription)
        {
            if (areNoticesSkipped)
            {
                return;
            }

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Log(LogTypeNotice, loggerSection, errorDescription);
        }

        public static void LogError(string loggerSection, string errorDescription)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log(LogTypeError, loggerSection, errorDescription);
        }

        public static void LogCritical(string loggerSection, string criticalDescription)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Log(LogTypeCritical, loggerSection, criticalDescription);
        }

        private static void Log(string logType, string loggerSection, string logDescription)
        {
            Debug.Log($"{loggerSection} {logType} - {logDescription}");
        }
    }
}
