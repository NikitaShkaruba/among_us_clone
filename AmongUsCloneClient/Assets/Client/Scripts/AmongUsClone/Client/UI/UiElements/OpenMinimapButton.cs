using AmongUsClone.Client.Game;
using UnityEngine;

namespace AmongUsClone.Client.UI.UiElements
{
    public class OpenMinimapButton : MonoBehaviour
    {
        public InputReader inputReader;

        public GameObject minimap;

        private bool isLocked;

        private void OnEnable()
        {
            inputReader.onToggleMinimap += Toggle;
        }

        private void OnDisable()
        {
            inputReader.onToggleMinimap -= Toggle;
        }

        public void Toggle()
        {
            minimap.SetActive(!minimap.activeSelf);
        }
    }
}
