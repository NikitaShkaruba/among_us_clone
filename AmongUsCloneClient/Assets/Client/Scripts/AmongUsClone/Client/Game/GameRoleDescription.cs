using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game
{
    public class GameRoleDescription : MonoBehaviour
    {
        [SerializeField] private Text roleDescriptionLabel;

        public void Start()
        {
            roleDescriptionLabel.text = GetLabelText();
        }

        private static string GetLabelText()
        {
            if (!GameManager.instance.controlledPlayer.information.isImposter)
            {
                return GameManager.instance.impostorsAmount == 1 ? "There is an impostor among us" : $"There are {GameManager.instance.impostorsAmount} impostors among us";
            }

            List<int> otherImpostorPlayerIds = new List<int>();
            foreach (int impostorPlayerId in GameManager.instance.impostorPlayerIds)
            {
                if (impostorPlayerId != GameManager.instance.controlledPlayer.information.id)
                {
                    otherImpostorPlayerIds.Add(impostorPlayerId);
                }
            }

            string labelText = "You are an impostor.";
            if (otherImpostorPlayerIds.Count != 0)
            {
                labelText += $" Other impostor player ids: {string.Join(", ", otherImpostorPlayerIds)}";
            }

            return labelText;
        }
    }
}
