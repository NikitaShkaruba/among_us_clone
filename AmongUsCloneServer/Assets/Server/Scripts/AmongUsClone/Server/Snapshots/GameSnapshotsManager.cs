using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server.Snapshots
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "GameSnapshotsManager", menuName = "GameSnapshotsManager")]
    public class GameSnapshotsManager : MonoBehaviour
    {
        [SerializeField] private Game.PlayersManager playersManager;
        [SerializeField] private PacketsSender packetsSender;
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;

        private static readonly Dictionary<int, GameSnapshot> gameSnapshots = new Dictionary<int, GameSnapshot>();

        private void FixedUpdate()
        {
            metaMonoBehaviours.applicationCallbacks.SchedulePostFixedUpdateAction(ProcessSnapshot);
        }

        private void ProcessSnapshot()
        {
            GameSnapshot lastGameSnapshot = CaptureSnapshot();

            SaveSnapshot(lastGameSnapshot);

            if (playersManager.clients.Count != 0)
            {
                packetsSender.SendGameSnapshotPackets(lastGameSnapshot);
            }
        }

        private static void SaveSnapshot(GameSnapshot gameSnapshot)
        {
            gameSnapshots.Add(gameSnapshot.id, gameSnapshot);

            if (gameSnapshots.Count > 1000)
            {
                gameSnapshots.Remove(gameSnapshots.Keys.Min());
            }

            Logger.LogEvent(LoggerSection.GameSnapshots, $"Game snapshot captured {gameSnapshot}");
        }

        private GameSnapshot CaptureSnapshot()
        {
            int snapshotId = gameSnapshots.Count != 0 ? gameSnapshots.Keys.Max() + 1 : 0;
            Dictionary<int, SnapshotPlayerInfo> players = CaptureSnapshotPlayers();

            return new GameSnapshot(snapshotId, players);
        }

        private Dictionary<int, SnapshotPlayerInfo> CaptureSnapshotPlayers()
        {
            Dictionary<int, SnapshotPlayerInfo> snapshotPlayers = new Dictionary<int, SnapshotPlayerInfo>();

            if (playersManager.clients.Count == 0)
            {
                return snapshotPlayers;
            }

            foreach (Client client in playersManager.clients.Values.ToList())
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                snapshotPlayers[client.player.information.id] = new SnapshotPlayerInfo(
                    client.player.information.id,
                    client.player.transform.position,
                    client.player.controllable.playerInput
                );
            }

            return snapshotPlayers;
        }
    }
}
