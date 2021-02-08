using System.Collections;
using System.Linq;
using AmongUsClone.Server.Game.Interactions;
using AmongUsClone.Server.Game.Maps;
using AmongUsClone.Server.Game.Maps.Surveillance;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Shared.Game;
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
        public ServerSkeld serverSkeld;

        public const int SecondsForRoleExploration = GameConfiguration.SecondsForRoleExploration;

        public void Initialize()
        {
            serverSkeld = FindObjectOfType<ServerSkeld>();

            PlacePlayersIntoMeetingPositions();
            metaMonoBehaviours.coroutines.StartCoroutine(UnlockPlayerMovement());

            scenesManager.UnloadScene(Scene.Lobby);
        }

        private void PlacePlayersIntoMeetingPositions()
        {
            foreach (Client client in playersManager.clients.Values.ToList())
            {
                client.basePlayer.transform.parent = serverSkeld.sharedSkeld.playerSpawnable.playersContainer.transform;
                client.basePlayer.transform.position = serverSkeld.sharedSkeld.playerSpawnable.playerMeetingLocations[client.playerId].transform.position;
                client.basePlayer.movable.isDisabled = true;
            }
        }

        private IEnumerator UnlockPlayerMovement()
        {
            yield return new WaitForSeconds(SecondsForRoleExploration);

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                client.basePlayer.movable.isDisabled = false;
            }
        }

        public void InteractWithAdminPanel(int playerId)
        {
            Interactable interactable = playersManager.clients[playerId].serverPlayer.nearbyInteractableChooser.chosen;
            if (interactable == null || interactable.GetType() != typeof(AdminPanel))
            {
                Logger.LogError(LoggerSection.Interactions, $"Attempt to reveal admin panel map for a player {playerId} which does not stand nearby");
                return;
            }

            interactable.Interact(playerId);
        }
    }
}
