// I want a class to have either static or non static methods
// ReSharper disable MemberCanBeMadeStatic.Global

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AmongUsClone.Server.Game.PlayerLogic;
using AmongUsClone.Server.Logging;
using AmongUsClone.Server.Networking;
using AmongUsClone.Server.Networking.PacketManagers;
using AmongUsClone.Shared;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using GameObject = UnityEngine.GameObject;
using Logger = AmongUsClone.Shared.Logging.Logger;
using Random = System.Random;

namespace AmongUsClone.Server.Game
{
    // Todo: migrate all singletons to Scriptable Objects
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance; // Instance is needed in order to have inspector variables and call MonoBehaviour methods

        public Lobby lobby;
        public GameObject serverMovablePrefab;

        public const int MinRequiredPlayersAmountForGame = 4;
        public const int SecondsForGameLaunch = 5;

        private void Awake()
        {
            if (instance != null)
            {
                Logger.LogCritical(LoggerSection.Initialization, "Attempt to instantiate GameLogic second time");
            }

            instance = this;
        }

        public void ConnectPlayer(int playerId, string playerName)
        {
            Player player = lobby.playersContainable.AddPlayer(Vector3.zero, serverMovablePrefab).GetComponent<Player>();

            PlayerColor playerColor = PlayerColors.TakeFreeColor(playerId);
            bool isLobbyHost = IsLobbyHost(playerId);
            player.Initialize(playerId, playerName, playerColor, isLobbyHost); // Todo: add disconnect of everyone after player 0 leaves

            Server.clients[playerId].FinishInitialization(player);

            foreach (Client client in Server.clients.Values)
            {
                if (!client.IsFullyInitialized())
                {
                    continue;
                }

                // Connect existent players with the new client (including himself)
                PacketsSender.SendPlayerConnectedPacket(client.playerId, Server.clients[playerId].player);

                // Connect new player with each client (himself is already spawned)
                if (client.playerId != playerId)
                {
                    PacketsSender.SendPlayerConnectedPacket(playerId, client.player);
                }
            }
        }

        private static bool IsLobbyHost(int playerId)
        {
            return playerId == Server.MinPlayerId;
        }

        public void DisconnectPlayer(int playerId)
        {
            if (!Server.clients.ContainsKey(playerId))
            {
                Logger.LogNotice(LoggerSection.Connection, $"Skipping player {playerId} disconnect, because it is already disconnected");
                return;
            }

            Logger.LogEvent(LoggerSection.Connection, $"{Server.clients[playerId].GetTcpEndPoint()} has disconnected (player {playerId})");

            if (Server.clients[playerId].player.information.isLobbyHost)
            {
                PacketsSender.SendKickedPacket(playerId);
                foreach (int playerIdToRemove in Server.clients.Keys.ToList())
                {
                    RemovePlayerFromGame(playerIdToRemove);
                }

                Logger.LogEvent(LoggerSection.Connection, $"Removed every player, because the host player {playerId} has disconnected");
            }
            else
            {
                PacketsSender.SendPlayerDisconnectedPacket(playerId);
                RemovePlayerFromGame(playerId);
            }
        }

        public void SavePlayerInput(int playerId, PlayerInput playerInput)
        {
            Server.clients[playerId].player.remoteControllable.EnqueueInput(playerInput);
        }

        public void ChangePlayerColor(int playerId)
        {
            PlayerColor newPlayerColor = PlayerColors.SwitchToRandomColor(playerId);

            Server.clients[playerId].player.colorable.ChangeColor(newPlayerColor);
            PacketsSender.SendColorChanged(playerId, newPlayerColor);

            Logger.LogEvent(SharedLoggerSection.PlayerColors, $"Changed player {playerId} color to {Helpers.GetEnumName(newPlayerColor)}");
        }

        private static void RemovePlayerFromGame(int playerId)
        {
            PlayerColors.ReleasePlayerColor(playerId);
            Destroy(Server.clients[playerId].player.gameObject);
            Server.clients.Remove(playerId);
        }

        public void ScheduleGameStart()
        {
            StartCoroutine(StartGame());
        }

        private static IEnumerator StartGame()
        {
            yield return new WaitForSeconds(SecondsForGameLaunch);

            int[] impostorPlayerIds = GetImpostorPlayerIds();
            foreach (int impostorPlayerId in impostorPlayerIds)
            {
                Server.clients[impostorPlayerId].player.information.isImposter = true;
            }

            PacketsSender.SendGameStartedPacket(impostorPlayerIds);

            Logger.LogEvent(SharedLoggerSection.GameStart, $"Game has started. Impostors: {string.Join(", ", impostorPlayerIds)}");
        }

        private static int[] GetImpostorPlayerIds()
        {
            int impostorsAmount = Server.clients.Count > 7 ? 2 : 1;

            Random random = new Random();
            int[] playerIds = Server.clients.Keys.ToArray();
            int[] shuffledPlayerIds = playerIds.OrderBy(playerId => random.Next()).ToArray();

            return shuffledPlayerIds.Take(impostorsAmount).ToArray();
        }
    }
}
