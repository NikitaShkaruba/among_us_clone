using System.Linq;
using AmongUsClone.Server.Networking;
using TMPro;
using UnityEngine;

namespace AmongUsClone.Server.Game.UI.Debug
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LastProcessedInputInfo : MonoBehaviour
    {
        [SerializeField] private PlayersManager playersManager;
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

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                int lastInputId = client.serverPlayer.remoteControllable.lastProcessedInputId;
                labelText += $"Player {client.playerId}: {lastInputId} \n";
            }

            textMeshPro.text = labelText;
        }
    }
}
