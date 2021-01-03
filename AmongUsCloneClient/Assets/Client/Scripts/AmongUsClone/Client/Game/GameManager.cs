using System;
using System.Collections;
using System.Collections.Generic;
using AmongUsClone.Client.Game.PlayerLogic;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Networking;
using AmongUsClone.Client.Snapshots;
using AmongUsClone.Client.UI;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
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

        public Dictionary<int, Player> players = new Dictionary<int, Player>();

        public UserInterface userInterface;
        public MainMenu mainMenu;
        public Lobby lobby;
        public Player controlledPlayer;

        [SerializeField] public GameObject clientControllablePlayerPrefab;
        [SerializeField] public GameObject playerPrefab;

        private void Awake()
        {
            Logger.Initialize(new[] {LoggerSection.Network, LoggerSection.GameSnapshots}, true);
            InitializeSingleton();
        }

        private void InitializeSingleton()
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

            foreach (Player player in players.Values)
            {
                Destroy(player.gameObject);
            }

            players.Clear();

            connectionToServer.Disconnect();
        }

        public void AddPlayerToLobby(int playerId, string playerName, Vector2 playerPosition)
        {
            GameObject chosenPlayerPrefab = playerId == connectionToServer.myPlayerId ? clientControllablePlayerPrefab : playerPrefab;
            players[playerId] = lobby.AddPlayer(playerId, playerName, playerPosition, chosenPlayerPrefab).GetComponent<Player>();
        }

        public void RemovePlayerFromLobby(int playerId)
        {
            if (!players.ContainsKey(playerId))
            {
                throw new Exception("Unable to remove non existent player");
            }

            Destroy(players[playerId].gameObject);
            players.Remove(playerId);
        }

        public void UpdatePlayerPosition(int playerId, Vector2 playerPosition)
        {
            players[playerId].movable.Teleport(playerPosition);
        }

        public void ProcessGameSnapshotPacketWithLag(Packet packet)
        {
            StartCoroutine(ProcessGameSnapshotPacketWithLagCoroutine(packet));
        }

        public void RegisterControlledPlayer(Player player)
        {
            controlledPlayer = player;

            PlayerCamera playerCamera = lobby.GetComponentInChildren<PlayerCamera>();
            playerCamera.target = controlledPlayer.gameObject;
        }

        private static IEnumerator ProcessGameSnapshotPacketWithLagCoroutine(Packet packet)
        {
            // Simulate network lag
            float secondsToWait = NetworkingOptimizationTests.NetworkDelayInSeconds;
            yield return new WaitForSeconds(secondsToWait);

            int snapshotId = packet.ReadInt();
            int lastProcessedInputId = packet.ReadInt();

            Dictionary<int, SnapshotPlayerInfo> snapshotPlayerInfos = new Dictionary<int, SnapshotPlayerInfo>();
            int snapshotPlayersAmount = packet.ReadInt();
            for (int snapshotPlayerIndex = 0; snapshotPlayerIndex < snapshotPlayersAmount; snapshotPlayerIndex++)
            {
                int playerId = packet.ReadInt();
                Vector2 playerPosition = packet.ReadVector2();

                snapshotPlayerInfos[playerId] = new SnapshotPlayerInfo(playerId, playerPosition);
            }

            ClientGameSnapshot gameSnapshot = new ClientGameSnapshot(snapshotId, lastProcessedInputId, snapshotPlayerInfos);
            GameSnapshots.ProcessSnapshot(gameSnapshot);
        }
    }
}
