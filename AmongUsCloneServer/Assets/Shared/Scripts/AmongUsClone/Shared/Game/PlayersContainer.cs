using UnityEngine;

namespace AmongUsClone.Shared.Game
{
    public class PlayersContainer : MonoBehaviour
    {
        public void PlacePlayerIntoPlayersContainer(GameObject playerGameObject)
        {
            playerGameObject.transform.parent = transform;
        }
    }
}