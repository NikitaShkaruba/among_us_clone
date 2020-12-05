using System;
using System.Collections.Generic;

namespace AmongUsClone.Shared
{
    /**
     * Class that handles multithreading
     */
    public static class ThreadManager
    {
        private static readonly List<Action> actionsToExecuteOnMainThread = new List<Action>();
        private static readonly List<Action> copyOfActionsToExecuteCopiedOnMainThread = new List<Action>();
        private static bool executionNeeded;

        /**
         * Sets an action to be executed on the main thread
         */
        public static void ExecuteOnMainThread(Action action)
        {
            lock (actionsToExecuteOnMainThread)
            {
                actionsToExecuteOnMainThread.Add(action);
                executionNeeded = true;
            }
        }

        /**
         * Executes all code meant to run on the main thread
         *
         * NOTE: Call this ONLY from the main thread
         */
        public static void ExecuteMainThreadActions()
        {
            if (!executionNeeded)
            {
                return;
            }

            copyOfActionsToExecuteCopiedOnMainThread.Clear();
            lock (actionsToExecuteOnMainThread)
            {
                copyOfActionsToExecuteCopiedOnMainThread.AddRange(actionsToExecuteOnMainThread);
                actionsToExecuteOnMainThread.Clear();
                executionNeeded = false;
            }

            foreach (Action action in copyOfActionsToExecuteCopiedOnMainThread)
            {
                action();
            }
        }
    }
}
