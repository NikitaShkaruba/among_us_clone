using System;
using System.Collections.Generic;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Shared.Game.Rooms
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class WalkingPlayersDetectable : MonoBehaviour
    {
        [SerializeField] private Room room;

        private readonly Dictionary<int, Action<Room>> onEnterEvents = new Dictionary<int, Action<Room>>();
        private readonly Dictionary<int, Action<Room>> onExitEvents = new Dictionary<int, Action<Room>>();
        private Action<Room, int> onAnyPlayerEnter;
        private Action<Room, int> onAnyPlayerExit;

        private void OnTriggerEnter2D(Collider2D other)
        {
            // Todo: migrate to separate player component
            PlayerInformation playerInformation = other.GetComponent<PlayerInformation>();
            if (playerInformation == null)
            {
                return;
            }

            onAnyPlayerEnter?.Invoke(room, playerInformation.id);

            if (onEnterEvents.ContainsKey(playerInformation.id))
            {
                onEnterEvents[playerInformation.id]?.Invoke(room);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            PlayerInformation playerInformation = other.GetComponent<PlayerInformation>();
            if (playerInformation == null)
            {
                return;
            }

            onAnyPlayerExit?.Invoke(room, playerInformation.id);

            if (onExitEvents.ContainsKey(playerInformation.id))
            {
                onExitEvents[playerInformation.id]?.Invoke(room);
            }
        }

        public void SubscribeToAnyPlayerEvents(Action<Room, int> onAnyPlayerEnter, Action<Room, int> onAnyPlayerExit)
        {
            this.onAnyPlayerEnter += onAnyPlayerEnter;
            this.onAnyPlayerExit += onAnyPlayerExit;
        }

        public void UnsubscribeFromAnyPlayerEvents(Action<Room, int> onAnyPlayerEnter, Action<Room, int> onAnyPlayerExit)
        {
            this.onAnyPlayerEnter -= onAnyPlayerEnter;
            this.onAnyPlayerExit -= onAnyPlayerExit;
        }

        public void SubscribeToPlayerEvents(int playerId, Action<Room> onEnter, Action<Room> onExit)
        {
            if (!onExitEvents.ContainsKey(playerId))
            {
                onEnterEvents[playerId] = delegate { };
                onExitEvents[playerId] = delegate { };
            }

            onEnterEvents[playerId] += onEnter;
            onExitEvents[playerId] += onExit;
        }

        public void UnsubscribeFromPlayerEvents(int playerId, Action<Room> onEnter, Action<Room> onExit)
        {
            onEnterEvents[playerId] -= onEnter;
            onExitEvents[playerId] -= onExit;
        }
    }
}
