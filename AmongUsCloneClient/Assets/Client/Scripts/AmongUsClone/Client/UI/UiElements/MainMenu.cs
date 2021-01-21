using System.Collections;
using AmongUsClone.Client.Game.GamePhaseManagers;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace AmongUsClone.Client.UI.UiElements
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;

        public InputField userNameField;
        public LoadingLabel loadingLabel;
        [HideInInspector] public bool isUserNameFieldHighlighted;

        public void ConnectToServer()
        {
            if (!IsUserNameFieldValid())
            {
                if (isUserNameFieldHighlighted)
                {
                    userNameField.text = GenerateRandomName();
                    Reset();
                }
                else
                {
                    HighlightUserNameField();
                    return;
                }
            }

            loadingLabel.gameObject.SetActive(true);
            mainMenuGamePhase.ConnectToLobby();
        }

        public void Reset()
        {
            userNameField.image.color = Color.white;
            isUserNameFieldHighlighted = false;
            loadingLabel.gameObject.SetActive(false);
        }

        private bool IsUserNameFieldValid()
        {
            return !userNameField.text.Trim().Equals("");
        }

        private void HighlightUserNameField()
        {
            isUserNameFieldHighlighted = true;
            userNameField.image.color = new Color(1f, 0.4858491f, 0.4858491f);
            StartCoroutine(WhitenUserNameField());
        }

        private IEnumerator WhitenUserNameField()
        {
            yield return new WaitForSeconds(0.01f);

            float otherColorsValue = userNameField.image.color.g + 0.01f <= 1f ? userNameField.image.color.g + 0.01f : 1f;
            userNameField.image.color = new Color(userNameField.image.color.r, otherColorsValue, otherColorsValue);

            if (userNameField.image.color == Color.white)
            {
                isUserNameFieldHighlighted = false;
            }
            else if (isUserNameFieldHighlighted)
            {
                StartCoroutine(WhitenUserNameField());
            }
        }

        private static string GenerateRandomName()
        {
            // Names of all the people we played original Among Us with during covid-19 2020 quarantine C:
            string[] randomNames =
            {
                "Nick",
                "Kristina",
                "VLAD",
                "Proskurin",
                "Vicky",
                "Andrey",
                "Ana",
                "sENjOY",
                "Pasha",
                "Lana",
                "Shuryak",
                "shvetsme"
            };

            int randomIndex = new Random((int) System.DateTime.Now.Ticks).Next(1, randomNames.Length - 1);
            return randomNames[randomIndex];
        }
    }
}
