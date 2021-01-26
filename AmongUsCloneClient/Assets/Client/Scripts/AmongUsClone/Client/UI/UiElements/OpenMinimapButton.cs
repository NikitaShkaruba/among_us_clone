using UnityEngine;

namespace AmongUsClone.Client.UI.UiElements
{
    public class OpenMinimapButton : MonoBehaviour
    {
        public GameObject minimap;

        private bool isLocked;

        public void Toggle()
        {
            minimap.SetActive(!minimap.activeSelf);
        }

        public void Click()
        {
            if (isLocked)
            {
                return;
            }

            isLocked = true;
            Toggle();
        }

        public void Release()
        {
            isLocked = false;
        }
    }
}
