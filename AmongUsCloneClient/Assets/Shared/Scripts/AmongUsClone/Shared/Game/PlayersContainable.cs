// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

using UnityEngine;

namespace AmongUsClone.Shared.Game
{
    public class PlayersContainable : MonoBehaviour
    {
        [SerializeField] private GameObject playersParentGameObject;

        public GameObject AddPlayer(Vector2 position, GameObject playerPrefab)
        {
            GameObject playerGameObject = Instantiate(playerPrefab, position, Quaternion.identity);
            playerGameObject.transform.parent = playersParentGameObject.transform;

            return playerGameObject;
        }
    }
}
