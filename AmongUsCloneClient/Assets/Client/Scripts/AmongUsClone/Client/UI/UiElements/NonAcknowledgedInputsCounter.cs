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
            if (GameManager.instance.controlledPlayer == null)
            {
                return;
            }

            ClientControllable clientControllable = GameManager.instance.controlledPlayer.GetComponent<ClientControllable>();
            int nonAcknowledgedInputsAmount = clientControllable.stateSnapshots.Count;
            textMeshPro.text = $"Non-acknowledged inputs: {nonAcknowledgedInputsAmount}";
        }
    }
}
