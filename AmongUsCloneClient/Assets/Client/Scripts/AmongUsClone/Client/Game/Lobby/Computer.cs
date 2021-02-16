using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Shared.Logging;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Lobby
{
    public class Computer : Interactable
    {
        public override void Interact()
        {
            Logger.LogEvent(SharedLoggerSection.PlayerColors, "Interacted with lobby computer");
        }
    }
}
