using AmongUsClone.Client.Game.PlayerLogic;
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
        [SerializeField] private Skeld skeld;

        public void Initialize()
        {
            skeld = FindObjectOfType<Skeld>();

            InitializePlayers();
            SetupCamera();

            ScenesManager.UnloadScene(Scene.RoleReveal);
        }

        private void InitializePlayers()
        {
            foreach (Player player in playersManager.players.Values)
            {
                player.transform.parent = skeld.playersContainer.transform;
                player.transform.position = skeld.playerMeetingLocations[player.information.id].transform.position;
                player.transform.localScale = new Vector3(1.7f, 1.7f, 0f); // Todo: remove this unconvenient 'scale back', 'name label activation' actions
                player.nameLabel.gameObject.SetActive(true);
                player.gameObject.SetActive(true);
            }

            playersManager.controlledPlayer.movable.isDisabled = false;
        }

        private void SetupCamera()
        {
            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();

            playerCamera.target = playersManager.controlledPlayer.gameObject;
            playerCamera.transform.position = Vector3.zero;
        }
    }
}
