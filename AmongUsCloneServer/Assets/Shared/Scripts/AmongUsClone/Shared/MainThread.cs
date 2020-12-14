// ReSharper disable UnusedMember.Global

using System;
using System.Collections.Generic;

namespace AmongUsClone.Shared
{
    /**
     * Class that handles multithreading
     */
    public static class MainThread
    {
        private static readonly List<Action> actionsToExecute = new List<Action>();
        private static readonly List<Action> actionsToExecuteCopy = new List<Action>();

        /**
         * Sets an action to be executed on the main thread
         */
        public static void ScheduleExecution(Action action)
        {
            lock (actionsToExecute)
            {
                actionsToExecute.Add(action);
            }
        }

        /**
         * Executes all code meant to run on the main thread
         *
         * NOTE: Call this ONLY from the main thread
         */
        public static void Execute()
        {
            // I don't know why resharper can't infer that this variable may never be null
            // ReSharper disable once InconsistentlySynchronizedField
            if (actionsToExecute.Count == 0)
            {
                return;
            }

            actionsToExecuteCopy.Clear();
            lock (actionsToExecute)
            {
                actionsToExecuteCopy.AddRange(actionsToExecute);
                actionsToExecute.Clear();
            }

            foreach (Action action in actionsToExecuteCopy)
            {
                action();
            }
        }
    }
}
