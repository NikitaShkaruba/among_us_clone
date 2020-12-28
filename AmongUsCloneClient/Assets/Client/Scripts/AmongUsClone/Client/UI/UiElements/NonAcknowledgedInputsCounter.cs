using AmongUsClone.Client.Game;
using AmongUsClone.Client.PlayerLogic;
using TMPro;
using UnityEngine;

namespace AmongUsClone.Client.UI.UiElements
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class NonAcknowledgedInputsCounter : MonoBehaviour
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
            // Todo: support multiple players
            if (!GameManager.instance.lobby.players.ContainsKey(0))
            {
                return;
            }

            ClientControllable clientControllable = GameManager.instance.lobby.players[0].GetComponent<ClientControllable>();
            int nonAcknowledgedInputsAmount = clientControllable.stateSnapshots.Count;
            textMeshPro.text = $"Non-acknowledged inputs: {nonAcknowledgedInputsAmount}";
        }
    }
}
