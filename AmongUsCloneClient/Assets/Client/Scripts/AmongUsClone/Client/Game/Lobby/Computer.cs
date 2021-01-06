using AmongUsClone.Client.Game.Interactions;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.Lobby
{
    public class Computer : Interactable
    {
        public Material materialWithOutline;
        public Material materialWithOutlineAndHighlight;
        public new Renderer renderer;

        public override void Interact()
        {
            Logger.LogDebug("Interaction happened");
        }

        protected override void SetHighlighting()
        {
            renderer.material = materialWithOutlineAndHighlight;
        }

        protected override void RemoveHighlighting()
        {
            renderer.material = materialWithOutline;
        }
    }
}
