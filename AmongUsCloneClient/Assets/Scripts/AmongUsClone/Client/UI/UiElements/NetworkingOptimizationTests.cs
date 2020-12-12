using AmongUsClone.Shared.Logging;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.UI.UiElements
{
    public class NetworkingOptimizationTests : MonoBehaviour
    {
        [SerializeField] private InputField lagInput;

        public static bool isPredictionEnabled;
        public static bool isReconciliationEnabled;
        public static bool isInterpolationEnabled;
        public static int millisecondsLag = 100;

        private void Awake()
        {
            lagInput.onValueChanged.AddListener(delegate { UpdateLag(); });
            lagInput.text = millisecondsLag.ToString();
        }

        public void ToggleServerPrediction(Toggle toggle)
        {
            isPredictionEnabled = toggle.isOn;
            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server prediction: {isPredictionEnabled}");
        }

        public void ToggleServerReconciliation(Toggle toggle)
        {
            isReconciliationEnabled = toggle.isOn;
            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server reconciliation: {isReconciliationEnabled}");
        }

        public void ToggleServerInterpolation(Toggle toggle)
        {
            isInterpolationEnabled = toggle.isOn;
            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server interpolation: {isInterpolationEnabled}");
        }

        private void UpdateLag()
        {
            millisecondsLag = int.Parse(lagInput.text);
        }
    }
}
