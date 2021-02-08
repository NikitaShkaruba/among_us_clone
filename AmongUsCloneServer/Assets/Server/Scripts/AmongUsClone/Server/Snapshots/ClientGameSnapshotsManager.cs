using System.Collections.Generic;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Networking;
using AmongUsClone.Shared.Scenes;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using Scene = AmongUsClone.Server.Game.Scene;

namespace AmongUsClone.Server.Snapshots
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "ClientGameSnapshotsManager", menuName = "ClientGameSnapshotsManager")]
    public class ClientGameSnapshotsManager : ScriptableObject
    {
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private PlayersManager playersManager;
        [SerializeField] private ScenesManager scenesManager;

        public ClientGameSnapshot CreateClientGameSnapshot(Client client, GameSnapshot gameSnapshot)
        {
            Dictionary<int, SnapshotPlayerInfo> visiblePlayersInfo = FilterHiddenPlayersFromSnapshot(gameSnapshot, client.playerId);
            int remoteControllableLastProcessedInputId = client.serverPlayer.remoteControllable.lastProcessedInputId;
            Dictionary<int, int> adminPanelInformation = IsAdminPanelInformationNeeded(client.playerId) ? playGamePhase.serverSkeld.adminPanel.GeneratePlayersData(client.playerId) : new Dictionary<int, int>();

            return new ClientGameSnapshot(gameSnapshot.id, remoteControllableLastProcessedInputId, visiblePlayersInfo, adminPanelInformation);
        }

        private bool IsAdminPanelInformationNeeded(int playerId)
        {
            if (scenesManager.GetActiveScene() != Scene.Skeld)
            {
                return false;
            }

            return playGamePhase.serverSkeld.adminPanel.IsPlayerLooking(playerId);
        }

        private Dictionary<int, SnapshotPlayerInfo> FilterHiddenPlayersFromSnapshot(GameSnapshot gameSnapshot, int currentPlayerId)
        {
            // Everyone sees everyone in the lobby
            if (scenesManager.GetActiveScene() == Scene.Lobby)
            {
                return gameSnapshot.playersInfo;
            }

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
                    bool isVisibleForCurrentPlayer = playersManager.clients[currentPlayerId].serverPlayer.viewable.visiblePlayers.Exists(player => player.basePlayer.information.id == snapshotPlayerInfo.id);
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
