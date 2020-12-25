using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using JetBrains.Annotations;

namespace AmongUsClone.Shared.Snapshots
{
    public static class GameSnapshotsDebug
    {
        private static string previousPlayerInformation = "";

        public static void Log(GameSnapshot lastGameSnapshot, [CanBeNull] Player playerToDebug)
        {
            string playerInformation = playerToDebug != null ? $" player: {{ position: {playerToDebug.movable.rigidbody.position}, controls: {playerToDebug.controllable.playerControls} }}" : "";

            if (previousPlayerInformation.Equals(playerInformation))
            {
                return;
            }

            previousPlayerInformation = playerInformation;
            Logger.LogDebug($"Snapshot #{lastGameSnapshot.id}{playerInformation}");
        }
    }
}
