// ReSharper disable UnusedMember.Global

using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    [RequireComponent(typeof(Movable))]
    [RequireComponent(typeof(Controllable))]
    public class Player : MonoBehaviour
    {
        public int id;
        public new string name;
        public Vector2 Position => movable.rigidbody.position;

        [HideInInspector] public Movable movable;
        [HideInInspector] public Controllable controllable;

        private void Awake()
        {
            movable = GetComponent<Movable>();
            controllable = GetComponent<Controllable>();
        }

        public void Initialize(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}
