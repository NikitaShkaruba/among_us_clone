using System.Collections.Generic;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Networking;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;

namespace AmongUsClone.Server.Snapshots
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "ClientGameSnapshotsManager", menuName = "ClientGameSnapshotsManager")]
    public class ClientGameSnapshotsManager : ScriptableObject
    {
        [SerializeField] private PlayersManager playersManager;

        public ClientGameSnapshot CreateClientGameSnapshot(Client client, GameSnapshot gameSnapshot)
        {
            Dictionary<int, SnapshotPlayerInfo> visiblePlayersInfo = FilterHiddenPlayersFromSnapshot(gameSnapshot, client.playerId);
            int remoteControllableLastProcessedInputId = client.player.remoteControllable.lastProcessedInputId;

            return new ClientGameSnapshot(gameSnapshot.id, visiblePlayersInfo, remoteControllableLastProcessedInputId);
        }

        private Dictionary<int, SnapshotPlayerInfo> FilterHiddenPlayersFromSnapshot(GameSnapshot gameSnapshot, int currentPlayerId)
        {
            Dictionary<int, SnapshotPlayerInfo> visibleSnapshotPlayersInfo = new Dictionary<int, SnapshotPlayerInfo>();

            foreach (Client client in playersManager.clients.Values)
            {
                foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo.Values)
                {
                    if (!client.IsFullyInitialized())
                    {
                        continue;
                    }

                    bool isSelfInformation = snapshotPlayerInfo.id == currentPlayerId;
                    bool isVisibleForCurrentPlayer = playersManager.clients[currentPlayerId].player.viewable.visiblePlayers.Exists(player => player.information.id == snapshotPlayerInfo.id);
                    if (!isVisibleForCurrentPlayer && !isSelfInformation)
                    {
                        continue;
                    }

                    visibleSnapshotPlayersInfo[snapshotPlayerInfo.id] = snapshotPlayerInfo;
                }
            }

            return visibleSnapshotPlayersInfo;
        }
    }
}
