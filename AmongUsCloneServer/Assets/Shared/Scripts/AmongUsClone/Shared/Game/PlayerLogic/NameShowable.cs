using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class NameShowable : MonoBehaviour
    {
        public Text textLabel;
        public BasePlayer basePlayer;

        public void Start()
        {
            textLabel.text = basePlayer.information.name;
        }
    }
}
