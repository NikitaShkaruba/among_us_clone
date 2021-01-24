using System.Collections;
using UnityEngine;

namespace AmongUsClone.Client.UI
{
    public class ActiveSceneUserInterface : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private float pauseMenuExpandSpeed;
        [SerializeField] private float pauseMenuUpdateRate;
        [SerializeField] private float pauseMenuMinScale;
        [SerializeField] private float pauseMenuMaxScale;

        public void TogglePauseMenu()
        {
            if (pauseMenu.activeSelf)
            {
                StartCoroutine(ShrinkToggleMenu());
            }
            else
            {
                pauseMenu.SetActive(true);
                StartCoroutine(ExpandToggleMenu());
            }
        }

        private IEnumerator ExpandToggleMenu()
        {
            yield return new WaitForSeconds(pauseMenuUpdateRate);

            pauseMenu.transform.localScale += new Vector3(pauseMenuExpandSpeed, pauseMenuExpandSpeed, 0f);

            if (pauseMenu.transform.localScale.x < pauseMenuMaxScale || pauseMenu.transform.localScale.y < pauseMenuMaxScale)
            {
                StartCoroutine(ExpandToggleMenu());
            }
        }

        private IEnumerator ShrinkToggleMenu()
        {
            yield return new WaitForSeconds(pauseMenuUpdateRate);

            pauseMenu.transform.localScale -= new Vector3(pauseMenuExpandSpeed, pauseMenuExpandSpeed, 0f);

            if (pauseMenu.transform.localScale.x > pauseMenuMinScale || pauseMenu.transform.localScale.y > pauseMenuMinScale)
            {
                StartCoroutine(ShrinkToggleMenu());
            }
            else
            {
                pauseMenu.SetActive(false);
            }
        }
    }
}
