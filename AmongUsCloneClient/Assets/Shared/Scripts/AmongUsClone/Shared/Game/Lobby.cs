// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMethodReturnValue.Global

using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Shared.Game
{
    public class Lobby : MonoBehaviour
    {
        [SerializeField] private GameObject playersParentGameObject;

        public GameObject AddPlayer(int playerId, string playerName, Vector2 playerPosition, GameObject playerPrefab)
        {
            GameObject playerGameObject = Instantiate(playerPrefab, new Vector3(playerPosition.x, playerPosition.y, 0), Quaternion.identity);
            playerGameObject.transform.parent = playersParentGameObject.transform;

            PlayerInformation playerInformation = playerGameObject.GetComponent<PlayerInformation>();
            playerInformation.Initialize(playerId, playerName);

            return playerGameObject;
        }
    }
}
