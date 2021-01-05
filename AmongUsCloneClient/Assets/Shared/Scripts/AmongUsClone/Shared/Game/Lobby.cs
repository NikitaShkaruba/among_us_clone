// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Shared.Game
{
    public class Lobby : MonoBehaviour
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
