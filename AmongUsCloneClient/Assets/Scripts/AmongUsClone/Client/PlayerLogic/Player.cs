using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    [RequireComponent(typeof(Movable))]
    public class Player : MonoBehaviour
    {
        // ReSharper disable once NotAccessedField.Global
        public int id;
        // ReSharper disable once NotAccessedField.Global
        public new string name;

        [HideInInspector] public Movable movable;

        private void Awake()
        {
            movable = GetComponent<Movable>();
        }
    }
}
