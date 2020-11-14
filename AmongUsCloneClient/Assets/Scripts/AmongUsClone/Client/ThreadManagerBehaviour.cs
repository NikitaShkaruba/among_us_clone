using AmongUsClone.Shared;
using UnityEngine;

namespace AmongUsClone.Client
{
    public class ThreadManagerBehaviour : MonoBehaviour
    {
        private void Update()
        {
            ThreadManager.UpdateMain();
        }
    }
}
