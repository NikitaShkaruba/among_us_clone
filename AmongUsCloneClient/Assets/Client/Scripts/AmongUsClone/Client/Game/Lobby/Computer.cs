using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Lobby
{
    public class Computer : Interactable
    {
        public PacketsSender packetsSender;

        public Material materialWithOutline;
        public Material materialWithOutlineAndHighlight;
        public new Renderer renderer;

        public override void Interact()
        {
            packetsSender.SendColorChangeRequestPacket();
            Logger.LogEvent(SharedLoggerSection.PlayerColors, "Sent request to change the color");
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
