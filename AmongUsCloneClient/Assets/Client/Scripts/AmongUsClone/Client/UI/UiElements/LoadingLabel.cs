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
                case ".":
                    return "..";

                case "..":
                    return "...";

                case "...":
                    return ".";

                default:
                    Logger.LogError(LoggerSection.MainMenu, "Undefined textLabel.text state");
                    return "";
            }
        }
    }
}
