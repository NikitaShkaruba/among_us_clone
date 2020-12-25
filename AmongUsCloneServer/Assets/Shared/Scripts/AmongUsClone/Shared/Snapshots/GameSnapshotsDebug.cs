using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using JetBrains.Annotations;

namespace AmongUsClone.Shared.Snapshots
{
    public static class GameSnapshotsDebug
    {
        public static void Log(GameSnapshot lastGameSnapshot, [CanBeNull] Player playerToDebug)
        {
            string message = $"Snapshot #{lastGameSnapshot.id}";

            if (playerToDebug != null)
            {
                message += $"player: {{position: {playerToDebug.movable.rigidbody.position}, controls: {playerToDebug.controllable.playerControls}}}";
            }

            Logger.LogDebug(message);
        }
    }
}
