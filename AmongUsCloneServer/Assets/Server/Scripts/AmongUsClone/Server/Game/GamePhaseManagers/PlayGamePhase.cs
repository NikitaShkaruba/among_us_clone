using System.Collections;
using System.Linq;
using AmongUsClone.Server.Networking;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.Maps;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;

namespace AmongUsClone.Server.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "PlayGamePhase", menuName = "PlayGamePhase")]
    public class PlayGamePhase : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private Skeld skeld;

        public const int SecondsForRoleExploration = GameConfiguration.SecondsForRoleExploration;

        public void Initialize()
        {
            skeld = FindObjectOfType<Skeld>();

            PlacePlayersIntoMeetingPositions();
            metaMonoBehaviours.coroutines.StartCoroutine(UnlockPlayerMovement());

            ScenesManager.UnloadScene(Scene.Lobby);
        }

        private void PlacePlayersIntoMeetingPositions()
        {
            foreach (Client client in playersManager.clients.Values.ToList())
            {
                client.player.transform.parent = skeld.playersContainer.transform;
                client.player.transform.position = skeld.playerMeetingLocations[client.playerId].transform.position;
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
    }
}
