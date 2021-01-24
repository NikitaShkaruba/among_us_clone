using AmongUsClone.Server.Game.GamePhaseManagers;
using AmongUsClone.Server.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = AmongUsClone.Shared.Logging.Logger;
using Scene = AmongUsClone.Server.Game.Scene;

namespace AmongUsClone.Server
{
    public class ServerInitializer : MonoBehaviour
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private ClientConnectionsListener clientConnectionsListener;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private PlayGamePhase playGamePhase;

        private void Start()
        {
            Logger.Initialize(new[] {LoggerSection.GameSnapshots, LoggerSection.Network}, true);
            Logger.LogEvent(LoggerSection.Initialization, "Started server initialization");

            scenesManager.Initialize(ScenesInitializationCallback);
            metaMonoBehaviours.Initialize();
            clientConnectionsListener.StartListening();

            scenesManager.LoadScene(Scenes.Lobby);
        }

        private void ScenesInitializationCallback(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadSceneMode)
        {
            switch (scene.name)
            {
                case Scene.Lobby:
                    lobbyGamePhase.Initialize();
                    break;

                case Scene.Skeld:
                    playGamePhase.Initialize();
                    break;
            }
        }
    }
}
