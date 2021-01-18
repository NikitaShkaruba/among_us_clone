using System;
using System.Collections.Generic;
using AmongUsClone.Client.Game.GamePhaseManagers;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game
{
    public class GameRoleDescription : MonoBehaviour
    {
        [SerializeField] private Text roleDescriptionLabel;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;

        public void Start()
        {
            roleDescriptionLabel.text = GetLabelText();
        }

        private string GetLabelText()
        {
                throw new NotImplementedException();
            // if (!lobbyGamePhase.controlledPlayer.information.isImposter)
            // {
                // return lobbyGamePhase.impostorsAmount == 1 ? "There is an impostor among us" : $"There are {GameManager.instance.impostorsAmount} impostors among us";
            // }

            // List<int> otherImpostorPlayerIds = new List<int>();
            // foreach (int impostorPlayerId in GameManager.instance.impostorPlayerIds)
            // {
                // if (impostorPlayerId != GameManager.instance.controlledPlayer.information.id)
                // {
                    // otherImpostorPlayerIds.Add(impostorPlayerId);
                // }
            // }

            // string labelText = "You are an impostor.";
            // if (otherImpostorPlayerIds.Count != 0)
            // {
                // labelText += $" Other impostor player ids: {string.Join(", ", otherImpostorPlayerIds)}";
            // }

            // return labelText;
        }
    }
}
