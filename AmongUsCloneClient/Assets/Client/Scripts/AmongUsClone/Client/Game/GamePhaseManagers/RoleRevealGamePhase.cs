using System.Collections;
using AmongUsClone.Client.Game.RoleReveal;
using AmongUsClone.Client.Meta;
using AmongUsClone.Shared.Game;
using AmongUsClone.Shared.Meta;
using AmongUsClone.Shared.Scenes;
using UnityEngine;

namespace AmongUsClone.Client.Game.GamePhaseManagers
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "RoleRevealGamePhase", menuName = "RoleRevealGamePhase")]
    public class RoleRevealGamePhase : ScriptableObject
    {
        [SerializeField] private MetaMonoBehaviours metaMonoBehaviours;
        [SerializeField] private ScenesManager scenesManager;
        [SerializeField] private RoleRevealScreen roleRevealScreen;

        private const int SecondsForRoleExploration = GameConfiguration.SecondsForRoleExploration;
        private const int SecondsBeforeRoleRevealing = GameConfiguration.SecondsForRoleExploration / 2;

        public void Initialize()
        {
            roleRevealScreen = FindObjectOfType<RoleRevealScreen>();

            metaMonoBehaviours.coroutines.StartCoroutine(RevealRole());
            FindObjectOfType<GameObjectsCache>().CachePlayers();
            RoleRevealScreen.UpdateCamera();

            scenesManager.UnloadScene(Scene.Lobby);
        }

        private IEnumerator RevealRole()
        {
            yield return new WaitForSeconds(SecondsBeforeRoleRevealing);

            roleRevealScreen.ShowRole();
            metaMonoBehaviours.coroutines.StartCoroutine(SwitchSceneToSkeld());
        }

        private IEnumerator SwitchSceneToSkeld()
        {
            yield return new WaitForSeconds(SecondsForRoleExploration);

            scenesManager.LoadScene(Scene.Skeld);
        }
    }
}
