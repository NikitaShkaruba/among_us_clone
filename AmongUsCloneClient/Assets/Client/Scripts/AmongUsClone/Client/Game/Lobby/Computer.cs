using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.Networking.PacketManagers;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Lobby
{
    public class Computer : Interactable
    {
        [Header("Scriptable objects")]
        public PacketsSender packetsSender;

        public override void Interact()
        {
            packetsSender.SendColorChangeRequestPacket();
            Logger.LogEvent(SharedLoggerSection.PlayerColors, "Sent request to change the color");
        }
    }
}
