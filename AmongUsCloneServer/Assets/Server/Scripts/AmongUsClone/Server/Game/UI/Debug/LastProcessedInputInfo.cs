using System.Collections.Generic;
using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Networking;
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

            foreach (Client client in Server.clients.Values)
            {
                int lastInputId = client.player.GetComponent<ServerPlayer>().lastProcessedInputId;
                labelText += $"Player {client.playerId}: {lastInputId} \n";
            }

            textMeshPro.text = labelText;
        }
    }
}
