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
            inputField.text = Application.targetFrameRate.ToString();
        }

        private void UpdateServerTicksPerSecond()
        {
            Application.targetFrameRate = int.Parse(inputField.text);
        }
    }
}
