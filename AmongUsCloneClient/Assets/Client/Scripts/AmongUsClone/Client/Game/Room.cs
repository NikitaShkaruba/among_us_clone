using System;
using AmongUsClone.Client.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Client.Game
{
    public class Room : MonoBehaviour
    {
        public PlayersManager playersManager;

        public new string name;

        public Action<string> onControlledPlayerEnter;
        public Action onControlledPlayerExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                return;
            }

            if (playersManager.controlledPlayer != player)
            {
                return;
            }

            onControlledPlayerEnter?.Invoke(name);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Player player = other.GetComponent<Player>();
            if (player == null)
            {
                return;
            }

            if (playersManager.controlledPlayer != player)
            {
                return;
            }

            onControlledPlayerExit?.Invoke();
        }
    }
}
