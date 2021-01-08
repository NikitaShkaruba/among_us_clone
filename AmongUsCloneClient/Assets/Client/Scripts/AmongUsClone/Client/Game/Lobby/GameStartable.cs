using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Lobby
{
    public class GameStartable : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image startGameButton;

        private void Awake()
        {
            GameManager.instance.playersAmountChanged += UpdateStartButtonOpacity;
        }

        private void OnDestroy()
        {
            GameManager.instance.playersAmountChanged -= UpdateStartButtonOpacity;
        }

        public void ShowStartButtonForHost()
        {
            startGameButton.gameObject.SetActive(true);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            StartGame();
        }

        private static void StartGame()
        {
            if (!GameManager.instance.HasEnoughPlayersForGame())
            {
                return;
            }

            Logger.LogDebug("Starting the game");
        }

        private void UpdateStartButtonOpacity()
        {
            startGameButton.color = GameManager.instance.HasEnoughPlayersForGame() ? Color.white : Helpers.halfVisibleColor;
        }
    }
}
