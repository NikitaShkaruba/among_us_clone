using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.UI.Buttons;
using UnityEngine;

namespace AmongUsClone.Client.UI
{
    public class PlayGamePhaseUserInterface : MonoBehaviour
    {
        [Header("Scriptable objects")]
        public PlayGamePhase playGamePhase;

        [Header("Parent element")]
        public ActiveSceneUserInterface activeSceneUserInterface;

        [Header("Own elements")]
        public MinimapButton minimapButton;

        private void OnEnable()
        {
            minimapButton.minimapToggled += ToggleActionButtons;
        }

        private void OnDisable()
        {
            minimapButton.minimapToggled -= ToggleActionButtons;
        }

        private void ToggleActionButtons(bool hide)
        {
            activeSceneUserInterface.interactButton.gameObject.SetActive(!hide);
        }
    }
}
