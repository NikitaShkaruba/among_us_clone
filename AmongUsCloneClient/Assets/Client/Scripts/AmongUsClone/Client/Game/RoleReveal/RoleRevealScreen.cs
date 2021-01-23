using System.Collections;
using AmongUsClone.Client.Game.PlayerLogic;
using UnityEngine;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

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
        [SerializeField] private PlayerDummy[] playerDummies;
        [SerializeField] private GameObject curtains;
        [SerializeField] private GameObject leftCurtain;
        [SerializeField] private GameObject rightCurtain;

        public float curtainOpeningSpeed = 0.1f;

        public void ShelterPlayerGameObjects()
        {
            foreach (Player player in playersManager.players.Values)
            {
                player.transform.parent = playersContainer.transform;
            }
        }

        public void ShowRole()
        {
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

            PlacePlayersIntoSlots(playersManager.controlledPlayer.information.isImposter);
            playersManager.controlledPlayer.movable.isDisabled = true;
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

        public void UpdateCamera()
        {
            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();

            playerCamera.target = null;
            playerCamera.transform.position = new Vector3(0, 0, -7);
            playerCamera.shaking = false;
        }

        private void PlacePlayersIntoSlots(bool impostorsOnly)
        {
            int playerDummyIndex = 0;

            for (int playerId = 0; playerId < playersManager.players.Count; playerId++)
            {
                Player player = playersManager.players[playerId];

                if (impostorsOnly && !player.information.isImposter)
                {
                    player.gameObject.SetActive(false);
                    continue;
                }

                ReplaceDummyWithPlayer(playerDummyIndex, player);

                player.nameLabel.gameObject.SetActive(false);

                playerDummyIndex++;
            }
        }

        private void ReplaceDummyWithPlayer(int playerDummyIndex, Player player)
        {
            PlayerDummy dummy = playerDummies[playerDummyIndex];

            player.transform.localPosition = dummy.transform.localPosition;
            player.transform.localScale = dummy.transform.localScale;
            player.spriteRenderer.flipX = dummy.spriteRenderer.flipX; // Todo: fix a bug with player flipX locking
            Logger.LogDebug($"Player {playerDummyIndex} sprite renderer's flip: {dummy.spriteRenderer.flipX}");
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
