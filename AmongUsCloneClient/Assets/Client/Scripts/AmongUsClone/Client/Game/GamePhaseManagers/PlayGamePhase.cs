using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared.Game.Maps;
using AmongUsClone.Shared.Scenes;
using UnityEngine;

namespace AmongUsClone.Client.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PlayGamePhase", menuName = "PlayGamePhase", order = 0)]
    public class PlayGamePhase : ScriptableObject
    {
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private Skeld skeld;

        public void Initialize()
        {
            skeld = FindObjectOfType<Skeld>();

            InitializePlayers();
            SetupCamera();

            scenesManager.UnloadScene(Scene.RoleReveal);
        }

        private void InitializePlayers()
        {
            foreach (Player player in playersManager.players.Values)
            {
                player.transform.parent = skeld.playersContainer.transform;
                player.transform.position = skeld.playerMeetingLocations[player.information.id].transform.position;

                if (player.information.isImposter)
                {
                    player.nameLabel.color = Color.red;
                }

                player.forciblyVisible.AllowHiding();
            }

            playersManager.controlledPlayer.viewable.Enable();
        }

        private void SetupCamera()
        {
            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();

            playerCamera.target = playersManager.controlledPlayer.gameObject;
            playerCamera.transform.position = Vector3.zero;
        }
    }
}
