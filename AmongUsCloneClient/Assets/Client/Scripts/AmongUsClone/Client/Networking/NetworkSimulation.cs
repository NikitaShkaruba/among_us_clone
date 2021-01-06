using System;
using System.Collections;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.UI.UiElements;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Networking
{
    /**
     * Class that simulates requests through network with lag. Because I'm testing on a local machine
     */
    public class NetworkSimulation : MonoBehaviour
    {
        public static NetworkSimulation instance;

        public void Awake()
        {
            if (instance != null)
            {
                Logger.LogCritical(LoggerSection.Initialization, "Attempt to instantiate singleton second time");
            }

            instance = this;
        }

        public void SendThroughNetwork(Action action)
        {
            StartCoroutine(ExecuteAfterNetworkDelay(action));
        }

        public void ReceiveThroughNetwork(Action action)
        {
            StartCoroutine(ExecuteAfterNetworkDelay(action));
        }

        private static IEnumerator ExecuteAfterNetworkDelay(Action action)
        {
            // Simulate network lag
            float secondsToWait = NetworkingOptimizationTests.NetworkDelayInSeconds;
            yield return new WaitForSeconds(secondsToWait);

            action();
        }
    }
}
