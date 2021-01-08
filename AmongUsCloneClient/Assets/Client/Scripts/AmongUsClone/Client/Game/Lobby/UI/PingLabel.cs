using AmongUsClone.Client.Networking;
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
            text.text = $"Ping: {NetworkSimulation.ping} ms";
        }
    }
}
