using System.Collections;
using System.Linq;
using AmongUsClone.Server.Game.Interactions;
using AmongUsClone.Server.Game.Maps.Surveillance;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Maps;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PlayGamePhase", menuName = "PlayGamePhase")]
    public class PlayGamePhase : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PlayerSpawnable playerSpawnable;

        public const int SecondsForRoleExploration = GameConfiguration.SecondsForRoleExploration;

        public void Initialize()
        {
            playerSpawnable = FindObjectOfType<PlayerSpawnable>();

            PlacePlayersIntoMeetingPositions();
            metaMonoBehaviours.coroutines.StartCoroutine(UnlockPlayerMovement());

            scenesManager.UnloadScene(Scene.Lobby);
        }

        private void PlacePlayersIntoMeetingPositions()
        {
            foreach (Client client in playersManager.clients.Values.ToList())
            {
                client.player.transform.parent = playerSpawnable.playersContainer.transform;
                client.player.transform.position = playerSpawnable.playerMeetingLocations[client.playerId].transform.position;
                client.player.movable.isDisabled = true;
            }
        }

        private IEnumerator UnlockPlayerMovement()
        {
            yield return new WaitForSeconds(SecondsForRoleExploration);

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                client.player.movable.isDisabled = false;
            }
        }

        public void RevealAdminPanelMap(int playerId)
        {
            Interactable interactable = playersManager.clients[playerId].player.nearbyInteractableChooser.chosen;
            if (interactable == null || interactable.GetType() != typeof(AdminPanel))
            {
                Logger.LogError(LoggerSection.Interactions, $"Attempt to reveal admin panel map for a player {playerId} which does not stand nearby");
                return;
            }

            interactable.Interact(playerId);
        }
    }
}
