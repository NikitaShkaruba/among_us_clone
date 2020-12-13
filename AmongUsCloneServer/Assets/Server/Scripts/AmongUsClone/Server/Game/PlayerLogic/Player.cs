using AmongUsClone.Shared;
using AmongUsClone.Shared.Game.PlayerLogic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        public int id;
        public new string name;
        public Vector2 Position => movable.rigidbody.position;

        private PlayerInput playerInput;
        public Movable movable;

        private void Awake()
        {
            movable = GetComponent<Movable>();
        }

        public void Initialize(int id, string name)
        {
            this.id = id;
            this.name = name;
            playerInput = new PlayerInput();
        }

        public void FixedUpdate()
        {
            movable.MoveByPlayerInput(playerInput);
        }

        public void UpdateInput(PlayerInput playerInput)
        {
            this.playerInput = playerInput;
        }
    }
}
