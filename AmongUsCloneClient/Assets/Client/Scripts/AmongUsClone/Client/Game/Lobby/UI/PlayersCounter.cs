using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.Lobby.UI
{
    public class PlayersCounter : MonoBehaviour
    {
        public Text textLabel;

        public void UpdatePlayerCounter(int playersAmount)
        {
            textLabel.text = $"{playersAmount} / 10";
            textLabel.color = playersAmount >= 4 ? Color.white : Color.red;
        }
    }
}
