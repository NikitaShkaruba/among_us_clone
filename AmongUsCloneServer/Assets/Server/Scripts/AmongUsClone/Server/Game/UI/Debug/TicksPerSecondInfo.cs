using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Server.Game.UI.Debug
{
    public class TicksPerSecondInfo : MonoBehaviour
    {
        [SerializeField] private InputField inputField;

        private void Start()
        {
            StartCoroutine(UpdateInitialServerTicksPerSecond());
            inputField.onValueChanged.AddListener(delegate { UpdateServerTicksPerSecond(); });
        }

        // I decided not to worry about c# events and just do it with this simple coroutine
        private IEnumerator UpdateInitialServerTicksPerSecond()
        {
            yield return new WaitForEndOfFrame();

            int ticksPerSecond = (int)Mathf.Floor(1 / Time.fixedDeltaTime);
            inputField.text = ticksPerSecond.ToString();
        }

        private void UpdateServerTicksPerSecond()
        {
            int ticksPerSecond = int.Parse(inputField.text);
            Time.fixedDeltaTime = 1 / (float)ticksPerSecond;
        }
    }
}
