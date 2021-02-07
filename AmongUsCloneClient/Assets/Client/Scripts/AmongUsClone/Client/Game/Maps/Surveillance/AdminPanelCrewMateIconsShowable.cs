using System.Collections.Generic;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Shared.Game.Rooms;
using UnityEngine;

namespace AmongUsClone.Client.Game.Maps.Surveillance
{
    public class AdminPanelCrewMateIconsShowable : MonoBehaviour
    {
        [Header("ScriptableObjects")]
        [SerializeField] private PlayGamePhase playGamePhase;

        [Header("Room icons")]
        [SerializeField] private GameObject[] cafeteriaIcons;
        [SerializeField] private GameObject[] weaponsIcons;
        [SerializeField] private GameObject[] o2Icons;
        [SerializeField] private GameObject[] navigationIcons;
        [SerializeField] private GameObject[] shieldsIcons;
        [SerializeField] private GameObject[] communicationsIcons;
        [SerializeField] private GameObject[] storageIcons;
        [SerializeField] private GameObject[] adminIcons;
        [SerializeField] private GameObject[] electricalIcons;
        [SerializeField] private GameObject[] lowerEngineIcons;
        [SerializeField] private GameObject[] reactorIcons;
        [SerializeField] private GameObject[] securityIcons;
        [SerializeField] private GameObject[] upperEngineIcons;
        [SerializeField] private GameObject[] medBayIcons;

        private GameObject[][] roomIcons;

        public void Start()
        {
            roomIcons = new[]
            {
                cafeteriaIcons,
                weaponsIcons,
                o2Icons,
                navigationIcons,
                shieldsIcons,
                communicationsIcons,
                storageIcons,
                adminIcons,
                electricalIcons,
                lowerEngineIcons,
                reactorIcons,
                securityIcons,
                upperEngineIcons,
                medBayIcons
            };
        }

        public void UpdateIcons(Dictionary<int, int> gameSnapshotAdminPanelPositions)
        {
            foreach (Room room in playGamePhase.clientSkeld.sharedSkeld.rooms)
            {
                int gameSnapshotAdminPanelPosition = gameSnapshotAdminPanelPositions.ContainsKey(room.id) ? gameSnapshotAdminPanelPositions[room.id] : 0;
                UpdateRoomIcons(room.id, gameSnapshotAdminPanelPosition);
            }
        }

        private void UpdateRoomIcons(int roomIndex, int neededIconsAmount)
        {
            int currentRoomActiveIconsAmount = GetCurrentRoomActiveIconsAmount(neededIconsAmount);

            if (neededIconsAmount == currentRoomActiveIconsAmount)
            {
                return;
            }

            ActivateIcons(roomIndex, neededIconsAmount);
        }

        private int GetCurrentRoomActiveIconsAmount(int roomId)
        {
            int currentRoomActiveIconsAmount = 0;

            foreach (GameObject crewMateIcon in roomIcons[roomId])
            {
                if (crewMateIcon.activeSelf)
                {
                    continue;
                }

                currentRoomActiveIconsAmount++;
            }

            return currentRoomActiveIconsAmount;
        }

        private void ActivateIcons(int roomIndex, int iconsAmountToActive)
        {
            for (int iconIndex = 0; iconIndex < roomIcons[roomIndex].Length; iconIndex++)
            {
                GameObject roomIcon = roomIcons[roomIndex][iconIndex];
                bool needsActivation = iconIndex < iconsAmountToActive;

                // Future iterations are useless
                if (!roomIcon.activeSelf && !needsActivation)
                {
                    break;
                }

                roomIcon.SetActive(needsActivation);
            }
        }
    }
}
