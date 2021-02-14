using UnityEngine;

namespace AmongUsClone.Client.Game.Interactions
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "InteractableHighlightMaterial", menuName = "InteractableHighlightMaterial", order = 0)]
    public class InteractableHighlightMaterials : ScriptableObject
    {
        public Material materialWithOutline;
        public Material materialWithOutlineAndHighlight;
    }
}
