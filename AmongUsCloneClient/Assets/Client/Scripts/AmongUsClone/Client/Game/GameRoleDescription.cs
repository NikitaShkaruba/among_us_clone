using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game
{
    public class GameRoleDescription : MonoBehaviour
    {
        [SerializeField] private Text roleDescriptionLabel;
        [SerializeField] private PlayersManager playersManager;

        public void Start()
        {
            roleDescriptionLabel.text = GetLabelText();
        }

        private string GetLabelText()
        {
            if (!playersManager.controlledPlayer.information.isImposter)
            {
                return playersManager.impostorsAmount == 1 ? "There is an impostor among us" : $"There are {playersManager.impostorsAmount} impostors among us";
            }

            List<int> otherImpostorPlayerIds = new List<int>();
            foreach (int impostorPlayerId in playersManager.knownImpostorPlayerIds)
            {
                if (impostorPlayerId != playersManager.controlledPlayer.information.id)
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
