// ReSharper disable UnusedMember.Global

using System;
using System.Collections;
using System.Collections.Generic;
using AmongUsClone.Server;
using AmongUsClone.Server.Snapshots;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Shared.Meta
{
    /**
     * Class that handles multithreading
     */
    public class MainThread : MonoBehaviour {
        [SerializeField] private GameSnapshotsManager gameSnapshotsManager;
        [SerializeField] private ClientConnectionsListener clientConnectionsListener;

        private static readonly List<Action> actionsToExecute = new List<Action>();
        private static readonly List<Action> actionsToExecuteCopy = new List<Action>();

        private void Awake()
        {
            Logger.LogEvent(SharedLoggerSection.Initialization, "Main server thread has started.");
        }

        private void FixedUpdate()
        {
            ExecuteScheduledActions();
            StartCoroutine(PostFixedUpdate());
        }

        private void OnApplicationQuit()
        {
            clientConnectionsListener.StopListening();
        }

        private IEnumerator PostFixedUpdate()
        {
            yield return new WaitForFixedUpdate();

            gameSnapshotsManager.ProcessSnapshot();
        }

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
        private static void ExecuteScheduledActions()
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
