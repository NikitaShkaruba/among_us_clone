using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class Movable : MonoBehaviour
    {
        public new Rigidbody2D rigidbody;

        public void MoveRelative(Vector2 relativePosition)
        {
            Move(rigidbody.position + relativePosition);
        }

        public void Move(Vector2 newPosition)
        {
            rigidbody.MovePosition(newPosition);
        }
    }
}
