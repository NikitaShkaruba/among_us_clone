using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.UI.Buttons;
using UnityEngine;

namespace AmongUsClone.Client.UI
{
    public class PlayGamePhaseUserInterface : MonoBehaviour
    {
        [Header("Parent element")]
        public ActiveSceneUserInterface activeSceneUserInterface;

        [Header("Own elements")]
        public MinimapButton minimapButton;
    }
}
