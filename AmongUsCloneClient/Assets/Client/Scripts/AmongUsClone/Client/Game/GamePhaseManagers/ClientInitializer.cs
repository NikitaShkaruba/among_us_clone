using AmongUsClone.Client.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.GamePhaseManagers
{
    public class ClientInitializer : MonoBehaviour
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;

        public void Start()
        {
            // Todo: restore after debug
            // Logger.Initialize(new[] {LoggerSection.Network, LoggerSection.GameSnapshots}, true);
            Logger.LogEvent(LoggerSection.Initialization, "Started client initialization");

            ScenesManager.Initialize(SceneInitializeCallbacks);
            metaMonoBehaviours.Initialize();
            Logger.LogEvent(LoggerSection.Initialization, "Initialized meta mono behaviours");

            ScenesManager.LoadScene(Scenes.MainMenu);
        }

        private void SceneInitializeCallbacks(Scene scene, LoadSceneMode loadedSceneMode)
        {
            switch (scene.name)
            {
                case Scenes.MainMenu:
                    mainMenuGamePhase.Initialize();
                    break;
            }
        }
    }
}
