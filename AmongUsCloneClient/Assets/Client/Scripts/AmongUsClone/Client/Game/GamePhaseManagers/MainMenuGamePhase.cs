using AmongUsClone.Client.Logging;
using AmongUsClone.Client.Networking;
using AmongUsClone.Client.UI.UiElements;
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
        public MetaMonoBehaviours metaMonoBehaviours;
        public ConnectionToServer connectionToServer;

        public MainMenu mainMenu;

        public void Initialize()
        {
            mainMenu = FindObjectOfType<MainMenu>();

            if (mainMenu == null)
            {
                Logger.LogEvent(LoggerSection.MainMenu, "Unable to find MainMenu object in a scene");
            }

            Logger.LogDebug("Initialized main menu game phase");
        }

        public void ConnectToLobby()
        {
            Logger.LogEvent(LoggerSection.Connection, "Connecting to a server");
            connectionToServer.Connect();
            metaMonoBehaviours.applicationCallbacks.ScheduleOnApplicationQuitActions(OnApplicationQuit);
            ScenesManager.LoadScene(Scene.Lobby);
        }

        // Unity holds some data between running game instances, so we need to cleanup by hand
        private void OnApplicationQuit()
        {
            connectionToServer.Disconnect();
        }
    }
}
