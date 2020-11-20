using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager instance;

        public GameObject startMenu;
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

        public void ConnectToServer()
        {
            startMenu.SetActive(false);
            userNameField.interactable = false;
            Networking.Client.instance.ConnectToServer();
        }
    }
}
