using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AmongUsClone.Shared.Meta
{
    public class ApplicationCallbacks : MonoBehaviour
    {
        private readonly List<Action> fixedUpdateActions = new List<Action>();
        private readonly List<Action> postFixedUpdateActions = new List<Action>();
        private readonly List<Action> onApplicationQuitActions = new List<Action>();

        public void ScheduleFixedUpdateAction(Action action)
        {
            lock (fixedUpdateActions)
            {
                fixedUpdateActions.Add(action);
            }
        }

        public void SchedulePostFixedUpdateAction(Action action)
        {
            lock (postFixedUpdateActions)
            {
                postFixedUpdateActions.Add(action);
            }
        }

        public void ScheduleOnApplicationQuitActions(Action action)
        {
            lock (onApplicationQuitActions)
            {
                onApplicationQuitActions.Add(action);
            }
        }

        private void OnApplicationQuit()
        {
            lock (onApplicationQuitActions)
            {
                foreach (Action action in onApplicationQuitActions)
                {
                    action();
                }
            }
        }

        private void FixedUpdate()
        {
            StartCoroutine(PostFixedUpdate());
            ExecuteFixedUpdateActions();
        }

        private IEnumerator PostFixedUpdate()
        {
            yield return new WaitForFixedUpdate();

            lock (postFixedUpdateActions)
            {
                foreach (Action action in postFixedUpdateActions)
                {
                    action();
                }
            }
        }

        /**
         * Executes all code meant to run on the main thread
         *
         * NOTE: Call this ONLY from the main thread
         */
        private void ExecuteFixedUpdateActions()
        {
            lock (fixedUpdateActions)
            {
                // I don't know why resharper can't infer that this variable may never be null
                // ReSharper disable once InconsistentlySynchronizedField
                if (fixedUpdateActions.Count == 0)
                {
                    return;
                }

                foreach (Action action in fixedUpdateActions)
                {
                    action();
                }

                fixedUpdateActions.Clear();
            }
        }
    }
}
