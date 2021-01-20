using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class NameShowable : MonoBehaviour
    {
        public Text textLabel;
        public PlayerInformation playerInformation;

        public void Start()
        {
            textLabel.text = playerInformation.name;
        }
    }
}
