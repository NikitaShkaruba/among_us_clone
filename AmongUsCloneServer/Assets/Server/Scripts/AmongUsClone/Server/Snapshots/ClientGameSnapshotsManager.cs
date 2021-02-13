using System.Collections.Generic;
using AmongUsClone.Server.Game;
using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Game.Maps.Surveillance;
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
        [SerializeField] private ScenesManager scenesManager;

        public ClientGameSnapshot CreateClientGameSnapshot(Client client, GameSnapshot gameSnapshot)
        {
            Dictionary<int, SnapshotPlayerInfo> visiblePlayersInfo = FilterNotViewablePlayersFromSnapshot(gameSnapshot, client.playerId);
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

        private Dictionary<int, SnapshotPlayerInfo> FilterNotViewablePlayersFromSnapshot(GameSnapshot gameSnapshot, int currentPlayerId)
        {
            // Everyone sees everyone in the lobby
            if (scenesManager.GetActiveScene() == Scene.Lobby)
            {
                return gameSnapshot.playersInfo;
            }

            Dictionary<int, SnapshotPlayerInfo> visibleSnapshotPlayersInfo = new Dictionary<int, SnapshotPlayerInfo>();

            foreach (SnapshotPlayerInfo snapshotPlayerInfo in gameSnapshot.playersInfo.Values)
            {
                bool isSelfInformation = snapshotPlayerInfo.id == currentPlayerId;
                bool isTooFarAway = (gameSnapshot.playersInfo[currentPlayerId].position - snapshotPlayerInfo.position).magnitude > 8;
                SecurityPanel securityPanel = playGamePhase.serverSkeld.securityPanel;
                bool isSeenFromSecurityCameras = securityPanel.IsPlayerLooking(currentPlayerId) && securityPanel.IsSeenFromSecurityCameras(snapshotPlayerInfo.id);

                if (isTooFarAway && !isSelfInformation && !isSeenFromSecurityCameras)
                {
                    continue;
                }

                visibleSnapshotPlayersInfo[snapshotPlayerInfo.id] = snapshotPlayerInfo;
            }

            return visibleSnapshotPlayersInfo;
        }
    }
}
