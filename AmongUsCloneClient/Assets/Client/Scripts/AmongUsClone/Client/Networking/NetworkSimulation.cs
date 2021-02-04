using System;
using System.Collections;
using AmongUsClone.Client.Game.Meta;
using AmongUsClone.Shared.Meta;
using UnityEngine;

namespace AmongUsClone.Client.Networking
{
    /**
     * Class that simulates requests through network with lag. Because I'm testing on a local machine
     */
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "NetworkSimulation", menuName = "NetworkSimulation")]
    public class NetworkSimulation : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;

        public const int Ping = 100;

        public void SendThroughNetwork(Action action)
        {
            metaMonoBehaviours.coroutines.StartCoroutine(ExecuteAfterNetworkDelay(action));
        }

        public void ReceiveThroughNetwork(Action action)
        {
            metaMonoBehaviours.coroutines.StartCoroutine(ExecuteAfterNetworkDelay(action));
        }

        private static IEnumerator ExecuteAfterNetworkDelay(Action action)
        {
            // Simulate network lag
            const float secondsToWait = Ping * 0.001f / 2f;
            yield return new WaitForSeconds(secondsToWait);

            action();
        }
    }
}
