using System.Collections.Generic;
using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Maps.Surveillance
{
    // Todo: forbid hotkeys when in adminmap (just E)
    // Todo: forbid hotkeys when in minimap (just wasd + tab)
    // Todo: fix strange drag when activating admin panel (probably because of a server prediction)
    public class AdminPanel : Interactable
    {
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PacketsSender packetsSender;

        [SerializeField] private GameObject adminPanelMinimap;
        [SerializeField] private Material materialWithOutline;
        [SerializeField] private Material materialWithOutlineAndHighlight;
        [SerializeField] private new Renderer renderer;
        [SerializeField] private AdminPanelCrewMateIconsShowable adminPanelCrewMateIconsShowable;

        public bool isControlledPlayerViewing;

        public override void Interact()
        {
            isControlledPlayerViewing = !isControlledPlayerViewing;
            packetsSender.SendAdminPanelInteractionPacket();

            if (isControlledPlayerViewing)
            {
                adminPanelMinimap.SetActive(true);
                playersManager.controlledClientPlayer.basePlayer.movable.isDisabled = true;
            }
            else
            {
                adminPanelMinimap.SetActive(false);
                playersManager.controlledClientPlayer.basePlayer.movable.isDisabled = false;
            }

            Logger.LogEvent(SharedLoggerSection.PlayerColors, "Requested admin panel");
        }

        public void UpdateMinimap(Dictionary<int, int> gameSnapshotAdminPanelPositions)
        {
                adminPanelCrewMateIconsShowable.UpdateIcons(gameSnapshotAdminPanelPositions);
        }

        protected override void SetHighlighting()
        {
            renderer.material = materialWithOutlineAndHighlight;
        }

        protected override void RemoveHighlighting()
        {
            renderer.material = materialWithOutline;
        }
    }
}
