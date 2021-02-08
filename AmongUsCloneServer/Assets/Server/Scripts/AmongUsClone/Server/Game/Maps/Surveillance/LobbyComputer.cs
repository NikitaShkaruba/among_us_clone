using AmongUsClone.Server.Game.Interactions;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game.Maps.Surveillance
{
    public class LobbyComputer : Interactable
    {
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private PacketsSender packetsSender;

        /**
         * Callback that is getting called when player presses interaction key near interactable
         */
        public override void Interact(int playerId)
        {
            PlayerColor newPlayerColor = PlayerColors.SwitchToRandomColor(playerId);

            playersManager.clients[playerId].basePlayer.colorable.ChangeColor(newPlayerColor);
            packetsSender.SendColorChanged(playerId, newPlayerColor);

            Logger.LogEvent(SharedLoggerSection.PlayerColors, $"Changed player {playerId} color to {Helpers.GetEnumName(newPlayerColor)}");
        }
    }
}
