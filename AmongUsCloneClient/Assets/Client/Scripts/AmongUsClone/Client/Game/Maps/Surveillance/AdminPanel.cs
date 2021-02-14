using System;
using System.Collections.Generic;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Game.Interactions;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Maps.Surveillance
{
    // Todo: fix strange drag when activating admin panel (probably because of a server prediction). This will be fixed if we start sending packets with next inputs ids
    [RequireComponent(typeof(PlayersLockable))]
    public class AdminPanel : Interactable
    {
        [Header("Scriptable objects")]
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private PacketsSender packetsSender;

        [Header("Self fields")]
        [SerializeField] private GameObject adminPanelMinimap;
        [SerializeField] private AdminPanelCrewMateIconsShowable adminPanelCrewMateIconsShowable;
        [SerializeField] private PlayersLockable playersLockable;
        public bool isControlledPlayerViewing;

        public Action onInteraction;

        public override void Interact()
        {
            if (playGamePhase.clientSkeld.playGamePhaseUserInterface.minimapButton.IsMinimapShown)
            {
                return;
            }

            isControlledPlayerViewing = !isControlledPlayerViewing;
            packetsSender.SendAdminPanelInteractionPacket();

            if (isControlledPlayerViewing)
            {
                adminPanelMinimap.SetActive(true);
                playersLockable.Add(playersManager.controlledClientPlayer.basePlayer.information.id);
                Logger.LogEvent(SharedLoggerSection.Interactions, "Started viewing admin panel");
            }
            else
            {
                adminPanelMinimap.SetActive(false);
                playersLockable.Remove(playersManager.controlledClientPlayer.basePlayer.information.id);
                Logger.LogEvent(SharedLoggerSection.Interactions, "Stopped viewing admin panel");
            }

            onInteraction?.Invoke();
        }

        public void UpdateMinimap(Dictionary<int, int> gameSnapshotAdminPanelPositions)
        {
            adminPanelCrewMateIconsShowable.UpdateIcons(gameSnapshotAdminPanelPositions);
        }
    }
}
