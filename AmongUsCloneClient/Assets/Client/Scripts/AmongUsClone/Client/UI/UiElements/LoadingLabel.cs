using AmongUsClone.Client.Logging;
using UnityEngine;
using UnityEngine.UI;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.UI.UiElements
{
    public class LoadingLabel : MonoBehaviour
    {
        [SerializeField] private Text textLabel;

        private float timer;
        public float secondsBeforeUpdate;

        private const string TextLabelOneDot = ".";
        private const string TextLabelTwoDots = "..";
        private const string TextLabelThreeDots = "...";

        private void Awake()
        {
            textLabel.text = TextLabelOneDot;
        }

        public void Update()
        {
            timer += Time.deltaTime;

            if (timer > secondsBeforeUpdate)
            {
                textLabel.text = GetLabelText();
                timer = 0;
            }
        }

        private string GetLabelText()
        {
            switch (textLabel.text)
            {
                case TextLabelOneDot:
                    return TextLabelTwoDots;

                case TextLabelTwoDots:
                    return TextLabelThreeDots;

                case TextLabelThreeDots:
                    return TextLabelOneDot;

                default:
                    Logger.LogError(LoggerSection.MainMenu, "Undefined textLabel text");
                    return "";
            }
        }
    }
}
