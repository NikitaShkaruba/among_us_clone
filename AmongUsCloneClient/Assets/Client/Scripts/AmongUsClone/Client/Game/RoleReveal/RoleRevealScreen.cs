using System.Collections;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Shared.Game;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.RoleReveal
{
    public class RoleRevealScreen : MonoBehaviour
    {
        [SerializeField] private PlayersManager playersManager;

        [SerializeField] private GameObject shhhScreen;
        [SerializeField] private GameObject crewmateEnvironment;
        [SerializeField] private GameObject impostorEnvironment;
        [SerializeField] private Text impostorsAmountExplanationLabel;
        [SerializeField] private Text impostorsAmountLabel;
        [SerializeField] private GameObject playersContainer;
        [SerializeField] private Player[] playerDummies;
        [SerializeField] private GameObject curtains;
        [SerializeField] private GameObject leftCurtain;
        [SerializeField] private GameObject rightCurtain;

        public float curtainOpeningSpeed = 0.1f;

        public void ShowRole()
        {
            shhhScreen.SetActive(false);

            if (playersManager.controlledPlayer.information.isImposter)
            {
                impostorEnvironment.SetActive(true);
            }
            else
            {
                UpdateImpostorsAmountLabel();
                crewmateEnvironment.SetActive(true);
            }

            UpdatePlayers(playersManager.controlledPlayer.information.isImposter);
            playersContainer.SetActive(true);

            curtains.SetActive(true);
            StartCoroutine(OpenCurtain());
        }

        private IEnumerator OpenCurtain()
        {
            yield return new WaitForEndOfFrame();

            leftCurtain.transform.position -= new Vector3(curtainOpeningSpeed, 0f, 0f);
            rightCurtain.transform.position += new Vector3(curtainOpeningSpeed, 0f, 0f);

            StartCoroutine(OpenCurtain());
        }

        public static void UpdateCamera()
        {
            // Todo: Add different camera to every scene
            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();

            playerCamera.target = null;
            playerCamera.transform.position = new Vector3(0, 0, -7);
            playerCamera.shaking = false;
        }

        private void UpdatePlayers(bool impostorsOnly)
        {
            int playerDummyIndex = 0;

            // Update colors of active dummies
            for (int playerId = 0; playerId < playersManager.players.Count; playerId++)
            {
                Player player = playersManager.players[playerId];

                if (impostorsOnly && !player.information.isImposter)
                {
                    continue;
                }

                UpdateDummyColor(playerDummyIndex, player.colorable.color);
                playerDummyIndex++;
            }

            // Hide not active dummies
            while (playerDummyIndex < GameConfiguration.PlayersAmount)
            {
                playerDummies[playerDummyIndex].gameObject.SetActive(false);
                playerDummyIndex++;
            }
        }

        private void UpdateDummyColor(int playerDummyIndex, PlayerColor color)
        {
            playerDummies[playerDummyIndex].colorable.ChangeColor(color);
        }

        private void UpdateImpostorsAmountLabel()
        {
            if (playersManager.impostorsAmount == 1)
            {
                impostorsAmountExplanationLabel.text = "There is                   among us";
                impostorsAmountLabel.text = "an imposter";
            }
            else
            {
                impostorsAmountExplanationLabel.text = "There are                     among us";
                impostorsAmountLabel.text = $" {playersManager.impostorsAmount} impostors";
            }
        }
    }
}
