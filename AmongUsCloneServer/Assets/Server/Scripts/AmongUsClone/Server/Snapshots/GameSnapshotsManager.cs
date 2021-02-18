using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Game.GamePhaseManagers;
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
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;

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

            bool gameStarted = false;
            bool gameStarts = false;
            bool securityCamerasEnabled = false;
            int impostorsAmount = 0;

            if (lobbyGamePhase.lobby != null)
            {
                gameStarts = lobbyGamePhase.GameStarts;
            }

            if (playGamePhase.serverSkeld != null)
            {
                gameStarted = true;
                impostorsAmount = playersManager.GetImpostorPlayerIds().Count;
                securityCamerasEnabled = playGamePhase.serverSkeld.securityPanel.securityCamerasEnabled;
            }

            return new GameSnapshot(snapshotId, players, gameStarts, gameStarted, impostorsAmount, securityCamerasEnabled);
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

                snapshotPlayers[client.basePlayer.information.id] = new SnapshotPlayerInfo(
                    client.basePlayer.information.id,
                    client.basePlayer.transform.position,
                    client.basePlayer.controllable.playerInput,
                    client.basePlayer.colorable.color,
                    client.basePlayer.impostorable.isImpostor
                );
            }

            return snapshotPlayers;
        }
    }
}
