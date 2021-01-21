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
        [SerializeField] private ClientConnectionsListener clientConnectionsListener;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;

        private void Start()
        {
            Logger.Initialize(new[] {LoggerSection.GameSnapshots, LoggerSection.Network}, true);
            Logger.LogEvent(LoggerSection.Initialization, "Started server initialization");

            ScenesManager.Initialize(SceneInitializeCallbacks);
            metaMonoBehaviours.Initialize();
            clientConnectionsListener.StartListening();

            ScenesManager.LoadScene(Scenes.Lobby);
        }

        private void SceneInitializeCallbacks(UnityEngine.SceneManagement.Scene scene, LoadSceneMode loadSceneMode)
        {
            SceneManager.SetActiveScene(scene);

            switch (scene.name)
            {
                case Scene.Lobby:
                    lobbyGamePhase.Initialize();
                    break;
            }
        }
    }
}
