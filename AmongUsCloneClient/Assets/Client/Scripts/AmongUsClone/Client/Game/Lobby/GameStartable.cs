using System.Collections;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Lobby
{
    public class GameStartable : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image startGameButton;
        public Text gameStartsInLabel;

        private int secondsBeforeGameStartsLeft;

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
            RequestGameStart();
        }

        private static void RequestGameStart()
        {
            if (!GameManager.instance.HasEnoughPlayersForGame())
            {
                return;
            }

            PacketsSender.SendStartGamePacket();
        }

        public void LaunchGameStart()
        {
            secondsBeforeGameStartsLeft = GameManager.instance.secondsForGameLaunch;
            gameStartsInLabel.text = GetGameStartsInLabelText();
            gameStartsInLabel.gameObject.SetActive(true);
            StartCoroutine(ProcessGameStartTick());

            Logger.LogEvent(SharedLoggerSection.GameStart, "Starting the game");
        }

        private void UpdateStartButtonOpacity()
        {
            startGameButton.color = GameManager.instance.HasEnoughPlayersForGame() ? Color.white : Helpers.halfVisibleColor;
        }

        private string GetGameStartsInLabelText()
        {
            return $"Game starts in {secondsBeforeGameStartsLeft}...";
        }

        private IEnumerator ProcessGameStartTick()
        {
            yield return new WaitForSeconds(1);

            secondsBeforeGameStartsLeft--;

            if (secondsBeforeGameStartsLeft == 0)
            {
                yield break;
            }

            gameStartsInLabel.text = GetGameStartsInLabelText();
            StartCoroutine(ProcessGameStartTick());
        }
    }
}
