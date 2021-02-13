using System.Collections;
using UnityEngine;

namespace AmongUsClone.Client.Game.Maps.Surveillance
{
    public class SecurityCamera : MonoBehaviour
    {
        public GameObject cameraRedIndicator;
        public float blinkInterval;

        private bool isEnabled;

        public void Enable()
        {
            isEnabled = true;
            StartCoroutine(ToggleRedIndicator());
        }

        public void Disable()
        {
            isEnabled = false;
            cameraRedIndicator.SetActive(false);
        }

        // ReSharper disable once FunctionRecursiveOnAllPaths
        private IEnumerator ToggleRedIndicator()
        {
            yield return new WaitForSeconds(blinkInterval);

            if (!isEnabled)
            {
                yield break;
            }

            cameraRedIndicator.SetActive(!cameraRedIndicator.activeSelf);

            StartCoroutine(ToggleRedIndicator());
        }
    }
}
