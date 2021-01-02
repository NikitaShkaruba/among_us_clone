using AmongUsClone.Server.Networking;
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

            foreach (Client client in Server.clients.Values)
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                int lastInputId = client.player.remoteControllable.lastProcessedInputId;
                labelText += $"Player {client.playerId}: {lastInputId} \n";
            }

            textMeshPro.text = labelText;
        }
    }
}
