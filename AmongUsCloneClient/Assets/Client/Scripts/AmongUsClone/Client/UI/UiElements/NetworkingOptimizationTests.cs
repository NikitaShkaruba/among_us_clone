using AmongUsClone.Shared.Logging;
using UnityEngine;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.UI.UiElements
{
    public class NetworkingOptimizationTests : MonoBehaviour
    {
        [SerializeField] private InputField pingInput;
        [SerializeField] private Toggle predictionToggle;
        [SerializeField] private Toggle reconciliationToggle;

        private static int ping;
        public static bool isPredictionEnabled;
        public static bool isReconciliationEnabled;
        private static bool isInterpolationEnabled;

        public static float NetworkDelayInSeconds => ping * 0.001f / 2f;

        private void Awake()
        {
            pingInput.onValueChanged.AddListener(delegate { UpdatePing(); });

            UpdatePing();
            isPredictionEnabled = predictionToggle.isOn;
            isReconciliationEnabled = reconciliationToggle.isOn;
        }

        public void ToggleServerPrediction(Toggle toggle)
        {
            isPredictionEnabled = toggle.isOn;

            if (!isPredictionEnabled && reconciliationToggle.isOn)
            {
                isReconciliationEnabled = isPredictionEnabled;
                reconciliationToggle.isOn = isReconciliationEnabled;
            }

            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server prediction: {isPredictionEnabled}");
        }

        public void ToggleServerReconciliation(Toggle toggle)
        {
            isReconciliationEnabled = toggle.isOn;

            if (isReconciliationEnabled && !predictionToggle.isOn)
            {
                isPredictionEnabled = isReconciliationEnabled;
                predictionToggle.isOn = isPredictionEnabled;
            }

            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server reconciliation: {isReconciliationEnabled}");
        }

        public void ToggleServerInterpolation(Toggle toggle)
        {
            isInterpolationEnabled = toggle.isOn;
            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server interpolation: {isInterpolationEnabled}");
        }

        public void UpdatePing()
        {
            ping = int.Parse(pingInput.text);
        }
    }
}
