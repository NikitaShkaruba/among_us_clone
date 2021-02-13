using System.Collections.Generic;
using AmongUsClone.Server.Game.Interactions;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared.Game.Interactions;
using AmongUsClone.Shared.Game.Rooms;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game.Maps.Surveillance
{
    [RequireComponent(typeof(PlayersLockable))]
    public class AdminPanel : Interactable
    {
        [SerializeField] private PlayersLockable playersLockable;

        public Dictionary<int, List<int>> playerIdsInRooms = new Dictionary<int, List<int>>();

        private void OnEnable()
        {
            Room[] rooms = FindObjectsOfType<Room>();

            foreach (Room room in rooms)
            {
                room.walkingPlayersDetectable.SubscribeToAnyPlayerEvents(OnPlayerEntered, OnPlayerExited);
            }
        }

        private void OnDestroy()
        {
            Room[] rooms = FindObjectsOfType<Room>();

            foreach (Room room in rooms)
            {
                room.walkingPlayersDetectable.UnsubscribeFromAnyPlayerEvents(OnPlayerEntered, OnPlayerExited);
            }
        }

        public override void Interact(int playerId)
        {
            if (playersLockable.IsPlayerLocked(playerId))
            {
                playersLockable.Remove(playerId);
                Logger.LogEvent(LoggerSection.AdminPanelViewing, $"Player {playerId} stopped looking at admin panel");
            }
            else
            {
                playersLockable.Add(playerId);
                Logger.LogEvent(LoggerSection.AdminPanelViewing, $"Player {playerId} started looking at admin panel");
            }
        }

        public bool IsPlayerLooking(int playerId)
        {
            return playersLockable.IsPlayerLocked(playerId);
        }

        public Dictionary<int, int> GeneratePlayersData(int playerId)
        {
            if (!playersLockable.IsPlayerLocked(playerId))
            {
                Logger.LogError(LoggerSection.AdminPanelViewing, $"Unable to generate admin panel information for player {playerId}, because he is not viewing it right now");
                return null;
            }

            Dictionary<int, int> adminPanelData = new Dictionary<int, int>();
            foreach (KeyValuePair<int, List<int>> playerIdsInRoom in playerIdsInRooms)
            {
                if (playerIdsInRoom.Value.Count == 0)
                {
                    continue;
                }

                adminPanelData[playerIdsInRoom.Key] = playerIdsInRoom.Value.Count;
            }

            Logger.LogEvent(LoggerSection.AdminPanelViewing, $"Generated admin panel data for player {playerId}");

            return adminPanelData;
        }

        private void OnPlayerEntered(Room room, int playerId)
        {
            if (!playerIdsInRooms.ContainsKey(room.id))
            {
                playerIdsInRooms[room.id] = new List<int>();
            }

            playerIdsInRooms[room.id].Add(playerId);
        }

        private void OnPlayerExited(Room room, int playerId)
        {
            if (!playerIdsInRooms.ContainsKey(room.id))
            {
                playerIdsInRooms[room.id] = new List<int>();
            }

            playerIdsInRooms[room.id].Remove(playerId);
        }
    }
}
