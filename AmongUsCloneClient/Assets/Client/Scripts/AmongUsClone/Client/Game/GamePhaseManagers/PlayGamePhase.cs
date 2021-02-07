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

            InitializePlayers();
            SetupCamera();

            scenesManager.UnloadScene(Scene.RoleReveal);
        }

        private void InitializePlayers()
        {
            foreach (Player player in playersManager.players.Values)
            {
                player.transform.parent = clientSkeld.playerSpawnable.playersContainer.transform;
                player.transform.position = clientSkeld.playerSpawnable.playerMeetingLocations[player.information.id].transform.position;

                if (player.information.isImposter)
                {
                    player.nameLabel.color = Color.red;
                }

                player.forciblyVisible.AllowHiding();
            }

            playersManager.controlledPlayer.viewable.Enable();
            clientSkeld.interactButton.SetInteractor(playersManager.controlledPlayer.interactor);
        }

        private void SetupCamera()
        {
            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();

            playerCamera.target = playersManager.controlledPlayer.gameObject;
            playerCamera.transform.position = Vector3.zero;
        }

        public void UpdateAdminPanelMinimap(Dictionary<int, int> gameSnapshotAdminPanelPositions)
        {
            clientSkeld.adminPanel.UpdateMinimap(gameSnapshotAdminPanelPositions);
        }
    }
}
