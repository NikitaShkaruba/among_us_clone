using System.Collections.Generic;
using AmongUsClone.Client.Game.Maps;
using AmongUsClone.Client.Game.PlayerLogic;
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

        public ClientSkeld clientSkeld;

        public void Initialize()
        {
            clientSkeld = FindObjectOfType<ClientSkeld>();
            clientSkeld.playGamePhaseUserInterface.activeSceneUserInterface.interactButton.UpdateCallbacks();

            InitializePlayers();
            SetupCamera();

            scenesManager.UnloadScene(Scene.RoleReveal);
        }

        private void InitializePlayers()
        {
            foreach (ClientPlayer player in playersManager.players.Values)
            {
                player.transform.parent = clientSkeld.playerSpawnable.playersContainer.transform;
                player.transform.position = clientSkeld.playerSpawnable.playerMeetingLocations[player.basePlayer.information.id].transform.position;

                if (player.basePlayer.impostorable.isImpostor)
                {
                    player.nameLabel.color = Color.red;
                }

                player.forciblyVisible.AllowHiding();
            }

            playersManager.controlledClientPlayer.viewable.Enable();
            clientSkeld.interactButton.SetInteractor(playersManager.controlledClientPlayer.interactor);
        }

        private void SetupCamera()
        {
            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();

            playerCamera.target = playersManager.controlledClientPlayer.gameObject;
            playerCamera.transform.position = Vector3.zero;
        }

        public void UpdateAdminPanelMinimap(Dictionary<int, int> gameSnapshotAdminPanelPositions)
        {
            clientSkeld.adminPanel.UpdateMinimap(gameSnapshotAdminPanelPositions);
        }

        public void EnableSecurityCameras()
        {
            clientSkeld.securityPanel.EnableSecurityCameras();
        }

        public void DisableSecurityCameras()
        {
            clientSkeld.securityPanel.DisableSecurityCameras();
        }
    }
}
