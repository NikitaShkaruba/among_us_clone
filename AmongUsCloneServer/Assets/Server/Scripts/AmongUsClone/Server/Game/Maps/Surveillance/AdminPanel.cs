using AmongUsClone.Server.Game.Interactions;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game.Maps.Surveillance
{
    public class AdminPanel : Interactable
    {
        public override void Interact(int playerId)
        {
            // Todo: implement
            Logger.LogDebug($"Player {playerId} tries to access admin panel");
        }
    }
}
