// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeMadeStatic.Global


using System;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Shared.Logging;
using UnityEngine;
using UnityEngine.SceneManagement;
using Logger = AmongUsClone.Shared.Logging.Logger;
using Scene = AmongUsClone.Client.Game.Scene;

namespace AmongUsClone.Shared.Scenes
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "ScenesManager", menuName = "ScenesManager")]
    public class ScenesManager : ScriptableObject
    {
        [SerializeField] private MainMenuGamePhase mainMenuGamePhase;
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private RoleRevealGamePhase roleRevealGamePhase;
        [SerializeField] private PlayGamePhase playGamePhase;

        public Action onSceneUpdate;
        private Action customSceneLoadedCallback;

        public void Initialize()
        {
            SceneManager.sceneLoaded += (scene, loadSceneMode) =>
            {
                SceneManager.SetActiveScene(scene);

                InitializeScene(scene);

                if (customSceneLoadedCallback != null)
                {
                    CallCustomSceneLoadedCallback();
                }
            };
        }

        private void InitializeScene(UnityEngine.SceneManagement.Scene scene)
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
                default:
                    Logger.LogError(SharedLoggerSection.ScenesManager, $"No game phase initializer found for the scene {scene.name}");
                    break;
            }
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

            UnityEngine.SceneManagement.Scene sceneToUnload = SceneManager.GetActiveScene();

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
