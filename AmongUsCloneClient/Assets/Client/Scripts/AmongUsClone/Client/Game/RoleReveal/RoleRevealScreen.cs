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

        [SerializeField] private GameObject ShhhScreen;
        [SerializeField] private GameObject CrewmateEnvironment;
        [SerializeField] private GameObject ImpostorEnvironment;
        [SerializeField] private Text ImpostorsAmountExplanationLabel;
        [SerializeField] private Text ImpostorsAmountLabel;
        [SerializeField] private GameObject playersContainer;
        [SerializeField] private Player[] playerDummies;
        [SerializeField] private GameObject curtains;
        [SerializeField] private GameObject leftCurtain;
        [SerializeField] private GameObject rightCurtain;

        public float curtainOpeningSpeed = 0.1f;

        public void ShowRole()
        {
            // Todo: fix blocking controlled player movement
            ShhhScreen.SetActive(false);

            if (playersManager.controlledPlayer.information.isImposter)
            {
                ImpostorEnvironment.SetActive(true);
            }
            else
            {
                UpdateImpostorsAmountLabel();
                CrewmateEnvironment.SetActive(true);
            }

            UpdatePlayers(playersManager.controlledPlayer.information.isImposter);
            playersContainer.SetActive(true);

            curtains.SetActive(true);
            StartCoroutine(OpenCurtain());
        }

        private IEnumerator OpenCurtain()
        {
            yield return new WaitForSeconds(0.001f);

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
                ImpostorsAmountExplanationLabel.text = "There is                   among us";
                ImpostorsAmountLabel.text = "an imposter";
            }
            else
            {
                ImpostorsAmountExplanationLabel.text = "There are                     among us";
                ImpostorsAmountLabel.text = $" {playersManager.impostorsAmount} impostors";
            }
        }
    }
}
