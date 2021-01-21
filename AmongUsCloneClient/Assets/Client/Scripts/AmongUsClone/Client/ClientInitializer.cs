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
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private RoleRevealGamePhase roleRevealGamePhase;

        public void Start()
        {
            Logger.Initialize(new[] {LoggerSection.Network, LoggerSection.GameSnapshots}, true);
            Logger.LogEvent(LoggerSection.Initialization, "Started client initialization");

            ScenesManager.Initialize(SceneInitializeCallbacks);
            metaMonoBehaviours.Initialize();
            Logger.LogEvent(LoggerSection.Initialization, "Initialized meta mono behaviours");

            ScenesManager.LoadScene(Scene.MainMenu);
        }

        private void SceneInitializeCallbacks(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadedSceneMode)
        {
            SceneManager.SetActiveScene(scene);

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
            }
        }
    }
}
