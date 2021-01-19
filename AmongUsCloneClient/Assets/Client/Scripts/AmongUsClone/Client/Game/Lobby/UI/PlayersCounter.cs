using AmongUsClone.Client.Game.GamePhaseManagers;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.Lobby.UI
{
    public class PlayersCounter : MonoBehaviour
    {
        public Text textLabel;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private PlayersManager playersManager;

        private void Start()
        {
            UpdateLabel();
            playersManager.playersAmountChanged += UpdateLabel;
        }

        private void OnDestroy()
        {
            playersManager.playersAmountChanged -= UpdateLabel;
        }

        private void UpdateLabel()
        {
            textLabel.text = $"{playersManager.players.Count} / {lobbyGamePhase.maxPlayersAmount}";
            textLabel.color = lobbyGamePhase.HasEnoughPlayersForGame() ? Color.white : Color.red;
        }
    }
}
