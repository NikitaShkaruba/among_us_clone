using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = AmongUsClone.Shared.Logging.Logger;
using Scene = AmongUsClone.Client.Game.Scene;

namespace AmongUsClone.Client
{
    public class ClientInitializer : MonoBehaviour
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private RoleRevealGamePhase roleRevealGamePhase;
        [SerializeField] private PlayGamePhase playGamePhase;

        public void Start()
        {
            Logger.Initialize(new[] {LoggerSection.Network, LoggerSection.GameSnapshots}, true);
            Logger.LogEvent(LoggerSection.Initialization, "Started client initialization");

            scenesManager.Initialize(ScenesInitializationCallback);
            metaMonoBehaviours.Initialize();
            Logger.LogEvent(LoggerSection.Initialization, "Initialized meta mono behaviours");

            scenesManager.LoadScene(Scene.MainMenu);
        }

        private void ScenesInitializationCallback(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadedSceneMode)
        {
            switch (scene.name)
            {
                case Scene.MainMenu:
                    mainMenuGamePhase.Initialize();
                    break;
                case Scene.Lobby:
                    lobbyGamePhase.Initialize();
                    break;
                case Scene.RoleReveal:
                    roleRevealGamePhase.Initialize();
                    break;
                case Scene.Skeld:
                    playGamePhase.Initialize();
                    break;
            }
        }
    }
}
