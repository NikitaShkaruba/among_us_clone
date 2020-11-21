using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager instance;

        public InputField userNameField;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Debug.Log("Instance already exists, destroying the object!");
                Destroy(this);
            }
        }
    }
}
