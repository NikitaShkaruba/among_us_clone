using AmongUsClone.Server.Game.Interactions;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared.Game.Interactions;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Game.Maps.Surveillance
{
    [RequireComponent(typeof(PlayersLockable))]
    public class SecurityPanel : Interactable
    {
        [SerializeField] private PlayersManager playersManager;

        [SerializeField] private PlayersLockable playersLockable;

        [SerializeField] private Transform[] cameras;

        public int cameraViewDistance;
        public bool securityCamerasEnabled => playersLockable.IsSomeoneLocked();

        public override void Interact(int playerId)
        {
            if (playersLockable.IsPlayerLocked(playerId))
            {
                playersLockable.Remove(playerId);
                Logger.LogEvent(LoggerSection.SecurityPanelViewing, $"Player {playerId} has stopped looking at the security panel");
            }
            else
            {
                playersLockable.Add(playerId);
                Logger.LogEvent(LoggerSection.SecurityPanelViewing, $"Player {playerId} has started looking at the security panel");
            }
        }

        public bool IsPlayerLooking(int playerId)
        {
            return playersLockable.IsPlayerLocked(playerId);
        }

        public bool IsSeenFromSecurityCameras(int playerId)
        {
            Transform playerTransform = playersManager.clients[playerId].basePlayer.transform;

            foreach (Transform cameraTransform in cameras)
            {
                if ((cameraTransform.transform.position - playerTransform.transform.position).magnitude < cameraViewDistance)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
