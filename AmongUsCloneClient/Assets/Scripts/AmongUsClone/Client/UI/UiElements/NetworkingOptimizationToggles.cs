using AmongUsClone.Shared.Logging;
using UnityEngine;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.UI.UiElements
{
    public class NetworkingOptimizationToggles : MonoBehaviour
    {
        public void ToggleServerPrediction(Toggle toggle)
        {
            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server prediction: {toggle.isOn}");
        }

        public void ToggleServerReconciliation(Toggle toggle)
        {
            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server reconciliation: {toggle.isOn}");
        }

        public void ToggleServerInterpolation(Toggle toggle)
        {
            Logger.LogEvent(LoggerSection.Initialization, $"Toggled server interpolation: {toggle.isOn}");
        }
    }
}
