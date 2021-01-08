using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.Lobby.UI
{
    public class PlayersCounter : MonoBehaviour
    {
        public Text textLabel;

        private void Start()
        {
            UpdateLabel();
            GameManager.instance.playersAmountChanged += UpdateLabel;
        }

        private void OnDestroy()
        {
            GameManager.instance.playersAmountChanged -= UpdateLabel;
        }

        private void UpdateLabel()
        {
            textLabel.text = $"{GameManager.instance.players.Count} / {GameManager.instance.maxPlayersAmount}";
            textLabel.color = GameManager.instance.HasEnoughPlayersForGame() ? Color.white : Color.red;
        }
    }
}
