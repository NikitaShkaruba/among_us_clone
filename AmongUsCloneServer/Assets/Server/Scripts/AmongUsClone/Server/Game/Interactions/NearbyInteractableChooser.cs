using AmongUsClone.Shared.Game.PlayerLogic;
using AmongUsClone.Shared.Scenes;
using UnityEngine;

namespace AmongUsClone.Server.Game.Interactions
{
    public class NearbyInteractableChooser : NearbyMonoBehavioursChooser<Interactable>
    {
        [SerializeField] private ScenesManager scenesManager;

        private void OnEnable()
        {
            scenesManager.onSceneUpdate += CacheChosables;
        }

        private void OnDisable()
        {
            scenesManager.onSceneUpdate -= CacheChosables;
        }
    }
}
