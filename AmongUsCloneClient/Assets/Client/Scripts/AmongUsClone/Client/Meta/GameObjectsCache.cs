using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Client.Meta
{
    /**
     * This class can be used to store some GameObjects between scenes
     */
    public class GameObjectsCache : MonoBehaviour
    {
        [SerializeField] private PlayersManager playersManager;

        [SerializeField] private GameObject playersContainer;

        public void CachePlayers()
        {
            foreach (ClientPlayer player in playersManager.players.Values)
            {
                player.transform.parent = playersContainer.transform;
            }
        }
    }
}
