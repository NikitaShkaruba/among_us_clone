using UnityEngine;

namespace AmongUsClone.Shared.Meta
{
    // CreateAssetMenu commented because we don't want to have more then 1 scriptable object of this type
    // [CreateAssetMenu(fileName = "MetaMonoBehaviours", menuName = "MetaMonoBehaviours")]
    public class MetaMonoBehaviours : ScriptableObject
    {
        [HideInInspector] public ApplicationCallbacks applicationCallbacks;
        [HideInInspector] public Coroutines coroutines;

        public void Initialize()
        {
            applicationCallbacks = FindObjectOfType<ApplicationCallbacks>();
            coroutines = FindObjectOfType<Coroutines>();
        }
    }
}
