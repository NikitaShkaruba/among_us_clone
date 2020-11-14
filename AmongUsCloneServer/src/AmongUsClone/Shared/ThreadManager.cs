using System;
using System.Collections.Generic;
using AmongUsClone.Server.Infrastructure;

namespace AmongUsClone.Shared
{
    /**
     * Class that handles multithreading
     */
    public static class ThreadManager
    {
        private static readonly List<Action> ActionsToExecuteOnMainThread = new List<Action>();
        private static readonly List<Action> CopyOfActionsToExecuteCopiedOnMainThread = new List<Action>();
        private static bool executionNeeded;

        /**
         * Sets an action to be executed on the main thread
         */
        public static void ExecuteOnMainThread(Action action)
        {
            lock (ActionsToExecuteOnMainThread)
            {
                ActionsToExecuteOnMainThread.Add(action);
                executionNeeded = true;
            }
        }

        /**
         * Executes all code meant to run on the main thread
         * 
         * NOTE: Call this ONLY from the main thread
         */
        public static void UpdateMain()
        {
            if (!executionNeeded)
            {
                return;
            }

            CopyOfActionsToExecuteCopiedOnMainThread.Clear();
            lock (ActionsToExecuteOnMainThread)
            {
                CopyOfActionsToExecuteCopiedOnMainThread.AddRange(ActionsToExecuteOnMainThread);
                ActionsToExecuteOnMainThread.Clear();
                executionNeeded = false;
            }

            foreach (Action action in CopyOfActionsToExecuteCopiedOnMainThread)
            {
                action();
            }
        }
    }
}
