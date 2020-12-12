using UnityEngine;

namespace AmongUsClone.Client.UI
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;

        public void TogglePauseMenu()
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }

        public void RemovePauseMenu()
        {
            pauseMenu.SetActive(false);
        }
    }
}
