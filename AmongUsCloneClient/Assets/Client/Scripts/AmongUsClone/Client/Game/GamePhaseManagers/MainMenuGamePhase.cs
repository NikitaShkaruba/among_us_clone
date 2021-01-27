using System;
using System.Collections.Generic;
using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Networking;
using AmongUsClone.Client.UI.UiElements;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "MainMenuGamePhase", menuName = "MainMenuGamePhase")]
    public class MainMenuGamePhase : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private ScenesManager scenesManager;
        public ConnectionToServer connectionToServer;

        public MainMenu mainMenu;

        private List<Action> onSceneLoadedActions;
        private bool sceneLoadRequested;

        public void Initialize()
        {
            onSceneLoadedActions = new List<Action>();
            sceneLoadRequested = false;

            mainMenu = FindObjectOfType<MainMenu>();

            if (mainMenu == null)
            {
                Logger.LogEvent(LoggerSection.MainMenu, "Unable to find MainMenu object in a scene");
            }
        }

        public void ConnectToLobby()
        {
            Logger.LogEvent(LoggerSection.Connection, "Connecting to a server");
            connectionToServer.Connect();
            metaMonoBehaviours.applicationCallbacks.ScheduleOnApplicationQuitActions(OnApplicationQuit);
        }

        public void InitializeLobby(int playerId, string playerName, PlayerColor playerColor, Vector2 playerPosition, bool playerLookingRight, bool isPlayerHost)
        {
            // We cannot instantly load a scene and then add a player to it - this is made at the next frame.
            // In order to solve it, we switch a scene and pass a callback where all wanted players will be added
            onSceneLoadedActions.Add(() => lobbyGamePhase.AddPlayerToLobby(playerId, playerName, playerColor, playerPosition, playerLookingRight, isPlayerHost));

            if (!sceneLoadRequested)
            {
                scenesManager.SwitchScene(Scene.Lobby, OnLobbySceneLoaded);
                sceneLoadRequested = true;
            }
        }

        private void OnLobbySceneLoaded()
        {
            foreach (Action onSceneLoadAction in onSceneLoadedActions)
            {
                onSceneLoadAction();
            }

            sceneLoadRequested = false;
        }

        // Unity holds some data between running game instances, so we need to cleanup by hand
        private void OnApplicationQuit()
        {
            connectionToServer.Disconnect();
        }
    }
}
