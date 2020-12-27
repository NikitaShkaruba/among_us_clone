using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using JetBrains.Annotations;

namespace AmongUsClone.Shared.Snapshots
{
    public static class GameSnapshotsDebug
    {
        private static string previousPlayerInformation = "";

        public static void Log(GameSnapshot lastGameSnapshot, Player localPlayer)
        {
            string playerInformation = lastGameSnapshot.playersInfo.Count != 0 ? $"Snapshot player: {{ position: {lastGameSnapshot.playersInfo[0].position} }}. Local player: {{ position: {localPlayer.movable.rigidbody.position}, input: {localPlayer.controllable.playerInput} }}" : "";

            if (previousPlayerInformation.Equals(playerInformation))
            {
                return;
            }

            previousPlayerInformation = playerInformation;
            Logger.LogDebug($"Snapshot #{lastGameSnapshot.id}. YourLastProcessedInputId: {lastGameSnapshot.yourLastProcessedInputId}. {playerInformation}");
        }
    }
}
