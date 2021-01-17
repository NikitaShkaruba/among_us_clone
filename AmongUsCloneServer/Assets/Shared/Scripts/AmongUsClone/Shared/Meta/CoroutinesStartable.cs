using System.Collections;
using UnityEngine;

namespace AmongUsClone.Shared.Meta
{
    // Object which always exist in a scene, which can be used to start coroutines
    public class CoroutinesStartable : MonoBehaviour
    {
        public new void StartCoroutine(IEnumerator coroutine)
        {
            base.StartCoroutine(coroutine);
        }
    }
}
