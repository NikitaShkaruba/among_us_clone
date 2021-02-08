using System.Collections;
using AmongUsClone.Client.Game;
using UnityEngine;

namespace AmongUsClone.Client.UI
{
    public class SettingsButton : MonoBehaviour
    {
        [SerializeField] private InputReader inputReader;

        [SerializeField] private GameObject settingsMenu;
        [SerializeField] private float menuExpandSpeed;
        [SerializeField] private float menuUpdateRate;
        [SerializeField] private float menuMinScale;
        [SerializeField] private float menuMaxScale;

        private void OnEnable()
        {
            inputReader.onToggleSettings += ToggleMenu;
        }

        private void OnDisable()
        {
            inputReader.onToggleSettings -= ToggleMenu;
        }

        public void ToggleMenu()
        {
            if (settingsMenu.activeSelf)
            {
                StartCoroutine(ShrinkMenu());
            }
            else
            {
                settingsMenu.SetActive(true);
                StartCoroutine(ExpandMenu());
            }
        }

        private IEnumerator ExpandMenu()
        {
            yield return new WaitForSeconds(menuUpdateRate);

            settingsMenu.transform.localScale += new Vector3(menuExpandSpeed, menuExpandSpeed, 0f);

            if (settingsMenu.transform.localScale.x < menuMaxScale || settingsMenu.transform.localScale.y < menuMaxScale)
            {
                StartCoroutine(ExpandMenu());
            }
        }

        private IEnumerator ShrinkMenu()
        {
            yield return new WaitForSeconds(menuUpdateRate);

            settingsMenu.transform.localScale -= new Vector3(menuExpandSpeed, menuExpandSpeed, 0f);

            if (settingsMenu.transform.localScale.x > menuMinScale || settingsMenu.transform.localScale.y > menuMinScale)
            {
                StartCoroutine(ShrinkMenu());
            }
            else
            {
                settingsMenu.SetActive(false);
            }
        }
    }
}
