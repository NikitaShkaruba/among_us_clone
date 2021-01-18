using AmongUsClone.Client.Game.GamePhaseManagers;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.Lobby.UI
{
    public class PlayersCounter : MonoBehaviour
    {
        public Text textLabel;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;

        private void Start()
        {
            UpdateLabel();
            lobbyGamePhase.playersAmountChanged += UpdateLabel;
        }

        private void OnDestroy()
        {
            lobbyGamePhase.playersAmountChanged -= UpdateLabel;
        }

        private void UpdateLabel()
        {
            textLabel.text = $"{lobbyGamePhase.players.Count} / {lobbyGamePhase.maxPlayersAmount}";
            textLabel.color = lobbyGamePhase.HasEnoughPlayersForGame() ? Color.white : Color.red;
        }
    }
}
