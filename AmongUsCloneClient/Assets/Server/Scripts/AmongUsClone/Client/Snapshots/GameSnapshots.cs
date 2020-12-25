using AmongUsClone.Client.Game;
using AmongUsClone.Client.PlayerLogic;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Snapshots;

namespace AmongUsClone.Client.Snapshots
{
    public static class GameSnapshots
    {
        public static void ProcessSnapshot(GameSnapshot gameSnapshot)
        {
            // Todo: fix always playerId being always 1
            ClientControllable clientControllable = GameManager.instance.lobby.players[0].GetComponent<ClientControllable>();
            clientControllable.RemoveObsoleteRequests(gameSnapshot);

            foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo)
            {
                GameManager.instance.UpdatePlayerPosition(snapshotPlayerInfo.id, snapshotPlayerInfo.position);
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Updated game state with snapshot {gameSnapshot.id}");
            GameSnapshotsDebug.Log(gameSnapshot, GameManager.instance.lobby.players.Count != 0 ? GameManager.instance.lobby.players[0] : null);
        }
    }
}
