using System.Collections;
using System.Collections.Generic;
using AmongUsClone.Client.Networking;
using AmongUsClone.Client.Snapshots;
using AmongUsClone.Client.UI;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Logging;
using AmongUsClone.Shared.Networking;
using AmongUsClone.Shared.Snapshots;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public readonly ConnectionToServer connectionToServer = new ConnectionToServer();

        public UserInterface userInterface;
        public MainMenu mainMenu;
        public Lobby lobby;

        [SerializeField] public GameObject clientControllablePlayerPrefab;
        [SerializeField] public GameObject playerPrefab;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Logger.LogError(LoggerSection.Initialization, "Instance already exists, destroying the object!");
                Destroy(this);
            }
        }

        private void Update()
        {
            MainThread.Execute();
        }

        // Unity holds some data between running game instances, so we need to cleanup by hand
        private void OnApplicationQuit()
        {
            connectionToServer.Disconnect();
        }

        public void ConnectToLobby()
        {
            mainMenu.gameObject.SetActive(false);
            lobby.gameObject.SetActive(true);

            connectionToServer.Connect();
        }

        public void DisconnectFromLobby()
        {
            userInterface.RemovePauseMenu();
            mainMenu.gameObject.SetActive(true);
            lobby.gameObject.SetActive(false);
            lobby.Reset();

            connectionToServer.Disconnect();
        }

        public void AddPlayerToLobby(int playerId, string playerName, Vector2 playerPosition)
        {
            GameObject chosenPlayerPrefab = playerId == connectionToServer.myPlayerId ? clientControllablePlayerPrefab : playerPrefab;
            lobby.AddPlayer(playerId, playerName, playerPosition, chosenPlayerPrefab);
        }

        public void RemovePlayerFromLobby(int playerId)
        {
            lobby.RemovePlayer(playerId);
        }

        public void UpdatePlayerPosition(int playerId, Vector2 playerPosition)
        {
            lobby.UpdatePlayerPosition(playerId, playerPosition);
        }

        public void ProcessGameSnapshotPacketWithLag(Packet packet)
        {
            StartCoroutine(ProcessGameSnapshotPacketWithLagCoroutine(packet));
        }

        private static IEnumerator ProcessGameSnapshotPacketWithLagCoroutine(Packet packet)
        {
            // Simulate network lag
            float secondsToWait = NetworkingOptimizationTests.NetworkDelayInSeconds;
            yield return new WaitForSeconds(secondsToWait);

            int snapshotId = packet.ReadInt();
            int lastProcessedInputId = packet.ReadInt();

            List<SnapshotPlayerInfo> snapshotPlayerInfos = new List<SnapshotPlayerInfo>();
            int snapshotPlayersAmount = packet.ReadInt();
            for (int snapshotPlayerIndex = 0; snapshotPlayerIndex < snapshotPlayersAmount; snapshotPlayerIndex++)
            {
                int playerId = packet.ReadInt();
                Vector2 playerPosition = packet.ReadVector2();

                snapshotPlayerInfos.Add(new SnapshotPlayerInfo(playerId, playerPosition));
            }

            GameSnapshot gameSnapshot = new GameSnapshot(snapshotId, lastProcessedInputId, snapshotPlayerInfos);
            GameSnapshots.ProcessSnapshot(gameSnapshot);
        }

    }
}
