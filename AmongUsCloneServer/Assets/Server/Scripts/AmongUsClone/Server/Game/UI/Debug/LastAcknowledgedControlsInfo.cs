using AmongUsClone.Server.Snapshots;
using TMPro;
using UnityEngine;

namespace AmongUsClone.Server.Game.UI.Debug
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LastAcknowledgedControlsInfo : MonoBehaviour
    {
        private TextMeshProUGUI textMeshPro;

        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            string lastAcknowledgedInputsInformation = "Last acknowledged inputs:\n";

            foreach (int playerId in LastClientRequestIds.lastPlayerRequestIds.Keys)
            {
                int lastRequestId = LastClientRequestIds.lastPlayerRequestIds[playerId];
                lastAcknowledgedInputsInformation += $"Player {playerId}: {lastRequestId} \n";
            }

            textMeshPro.text = lastAcknowledgedInputsInformation;
        }
    }
}
