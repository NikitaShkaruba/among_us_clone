using AmongUsClone.Server.Logging;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Server
{
    public class ServerInitializer : MonoBehaviour
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private ClientConnectionsListener clientConnectionsListener;

        private void Start()
        {
            // Todo: restore after debug
            Logger.Initialize(new[] {LoggerSection.GameSnapshots}, true);
            Logger.LogEvent(LoggerSection.Initialization, "Started server initialization");

            metaMonoBehaviours.Initialize();
            clientConnectionsListener.StartListening();

            ScenesManager.LoadScene(Scenes.Lobby);
        }
    }
}
