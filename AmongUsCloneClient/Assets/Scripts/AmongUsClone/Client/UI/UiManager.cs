using AmongUsClone.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance;

        public GameObject startMenu;
        public InputField userNameField;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Debug.Log("Instance already exists, destroying the object!");
                Destroy(this);
            }
        }

        public void ConnectToServer()
        {
            startMenu.SetActive(false);
            userNameField.interactable = false;
            Networking.Client.Instance.ConnectToServer();
        }
    }
}
