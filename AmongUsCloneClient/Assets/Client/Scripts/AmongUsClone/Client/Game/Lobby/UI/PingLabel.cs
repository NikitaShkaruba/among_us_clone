using AmongUsClone.Client.UI.UiElements;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.Game.Lobby.UI
{
    [RequireComponent(typeof(Text))]
    public class PingLabel : MonoBehaviour
    {
        public Text text;

        public void Awake()
        {
            text.text = $"Ping: {NetworkingOptimizationTests.ping} ms";
        }
    }
}
