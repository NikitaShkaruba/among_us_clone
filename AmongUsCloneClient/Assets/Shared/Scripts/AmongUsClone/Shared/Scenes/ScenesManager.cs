// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeMadeStatic.Global

using System;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Shared.Scenes
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "ScenesManager", menuName = "ScenesManager")]
    public class ScenesManager : ScriptableObject
    {
        public Action onSceneUpdate;
        private Action customSceneLoadedCallback;

        public void Initialize(UnityAction<Scene, LoadSceneMode> defaultScenesInitializationCallback)
        {
            SceneManager.sceneLoaded += (scene, loadSceneMode) =>
            {
                SceneManager.SetActiveScene(scene);

                defaultScenesInitializationCallback(scene, loadSceneMode);

                if (customSceneLoadedCallback != null)
                {
                    CallCustomSceneLoadedCallback();
                }
            };
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
            Logger.LogEvent(SharedLoggerSection.ScenesManager, $"Loaded scene {sceneName}");

            onSceneUpdate?.Invoke();
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
            Logger.LogEvent(SharedLoggerSection.ScenesManager, $"Unloaded scene {sceneName}");

            onSceneUpdate?.Invoke();
        }

        public void SwitchScene(string sceneName, Action customSceneLoadedCallback = null)
        {
            this.customSceneLoadedCallback = customSceneLoadedCallback;

            Scene sceneToUnload = SceneManager.GetActiveScene();

            LoadScene(sceneName);
            UnloadScene(sceneToUnload.name);
        }

        public string GetActiveScene()
        {
            return SceneManager.GetActiveScene().name;
        }

        private void CallCustomSceneLoadedCallback()
        {
            customSceneLoadedCallback();
            customSceneLoadedCallback = null;
        }
    }
}
