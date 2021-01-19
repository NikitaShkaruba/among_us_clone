// ReSharper disable UnusedMember.Global

using AmongUsClone.Shared.Logging;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Shared.Scenes
{
    public static class ScenesManager
    {
        public static void Initialize(UnityAction<Scene, LoadSceneMode> sceneInitializeCallbacks)
        {
            SceneManager.sceneLoaded += sceneInitializeCallbacks;
        }

        public static void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            Logger.LogEvent(SharedLoggerSection.ScenesManager, $"Loaded scene {sceneName}");
        }

        public static void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Logger.LogEvent(SharedLoggerSection.ScenesManager, $"Unloaded scene {sceneName}");
        }

        public static void SwitchScene(string sceneName)
        {
            Scene sceneToUnload = SceneManager.GetActiveScene();

            LoadScene(sceneName);
            UnloadScene(sceneToUnload.name);
        }

        public static string GetActiveScene()
        {
            return SceneManager.GetActiveScene().name;
        }
    }
}
