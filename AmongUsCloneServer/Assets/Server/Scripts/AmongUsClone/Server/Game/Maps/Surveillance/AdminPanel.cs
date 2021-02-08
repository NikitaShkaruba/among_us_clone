using System.Collections.Generic;
using AmongUsClone.Server.Game.Interactions;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared.Game.Rooms;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game.Maps.Surveillance
{
    public class AdminPanel : Interactable
    {
        [SerializeField] private PlayersManager playersManager;

        public List<int> lookingPlayerIds;
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
            if (IsPlayerLooking(playerId))
            {
                lookingPlayerIds.Remove(playerId);
                playersManager.clients[playerId].basePlayer.movable.isDisabled = false;
                Logger.LogEvent(LoggerSection.AdminPanelViewing, $"Player {playerId} stopped looking at admin panel");
            }
            else
            {
                lookingPlayerIds.Add(playerId);
                playersManager.clients[playerId].basePlayer.movable.isDisabled = true;
                Logger.LogEvent(LoggerSection.AdminPanelViewing, $"Player {playerId} started looking at admin panel");
            }
        }

        public Dictionary<int, int> GeneratePlayersData(int playerId)
        {
            if (!IsPlayerLooking(playerId))
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

        public bool IsPlayerLooking(int playerId)
        {
            return lookingPlayerIds.Contains(playerId);
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
