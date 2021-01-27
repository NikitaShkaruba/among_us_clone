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

        public void Start()
        {
            Logger.Initialize(new[] {LoggerSection.Network, LoggerSection.GameSnapshots}, true);
            Logger.LogEvent(LoggerSection.Initialization, "Started client initialization");

            metaMonoBehaviours.Initialize();
            scenesManager.Initialize();
            Logger.LogEvent(LoggerSection.Initialization, "Initialized global environment");

            scenesManager.LoadScene(Scene.MainMenu);
        }
    }
}
