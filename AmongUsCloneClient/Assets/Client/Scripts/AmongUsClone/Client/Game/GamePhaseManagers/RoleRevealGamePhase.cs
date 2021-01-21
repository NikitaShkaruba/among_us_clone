using System.Collections;
using AmongUsClone.Client.Game.RoleReveal;
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
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private RoleRevealScreen roleRevealScreen;
        [SerializeField] private PlayersManager playersManager;

        public void Initialize()
        {
            roleRevealScreen = FindObjectOfType<RoleRevealScreen>();

            metaMonoBehaviours.coroutines.StartCoroutine(RevealRole());
            roleRevealScreen.ShelterPlayerGameObjects();
            roleRevealScreen.UpdateCamera();
            ScenesManager.UnloadScene(Scene.Lobby);
        }

        private IEnumerator RevealRole()
        {
            yield return new WaitForSeconds(3);

            roleRevealScreen.ShowRole();
        }
    }
}
