using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Maps.Surveillance
{
    public class AdminPanel : Interactable
    {
        [SerializeField] private PacketsSender packetsSender;

        public Material materialWithOutline;
        public Material materialWithOutlineAndHighlight;
        public new Renderer renderer;

        public override void Interact()
        {
            packetsSender.SendAdminPanelRequestPacket();
            Logger.LogEvent(SharedLoggerSection.PlayerColors, "Requested admin panel");
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
