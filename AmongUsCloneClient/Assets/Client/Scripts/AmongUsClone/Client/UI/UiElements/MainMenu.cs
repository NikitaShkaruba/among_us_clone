using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI.UiElements
{
    public class MainMenu : MonoBehaviour
    {
        public InputField userNameField;

        public bool IsUserNameFieldValid()
        {
            return !userNameField.text.Trim().Equals("");
        }

        public void HighlightUserNameField()
        {
            userNameField.image.color = Color.red;
            StartCoroutine(WhitenUserNameField());
        }

        private IEnumerator WhitenUserNameField()
        {
            yield return new WaitForSeconds(0.01f);

            float otherColorsValue = userNameField.image.color.g + 0.01f <= 1f ? userNameField.image.color.g + 0.01f : 1f;
            userNameField.image.color = new Color(userNameField.image.color.r, otherColorsValue, otherColorsValue);

            if (userNameField.image.color != Color.white)
            {
                StartCoroutine(WhitenUserNameField());
            }
        }
    }
}
