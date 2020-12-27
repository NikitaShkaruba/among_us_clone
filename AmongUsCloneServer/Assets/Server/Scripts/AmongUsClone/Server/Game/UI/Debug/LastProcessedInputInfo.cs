using AmongUsClone.Server.Snapshots;
using TMPro;
using UnityEngine;

namespace AmongUsClone.Server.Game.UI.Debug
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LastProcessedInputInfo : MonoBehaviour
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
            string labelText = "Last processed inputs:\n";

            foreach (int playerId in ProcessedPlayerInputs.lastPlayersProcessedInputIds.Keys)
            {
                int lastInputId = ProcessedPlayerInputs.lastPlayersProcessedInputIds[playerId];
                labelText += $"Player {playerId}: {lastInputId} \n";
            }

            textMeshPro.text = labelText;
        }
    }
}
