using UnityEngine;

namespace AmongUsClone.Client.PlayerLogic
{
    public class Movable : MonoBehaviour
    {
        public new Rigidbody2D rigidbody;

        public void RelativeMove(Vector2 relativePosition)
        {
            Move(rigidbody.position + relativePosition);
        }

        public void Move(Vector2 newPosition)
        {
            rigidbody.MovePosition(newPosition);
        }
    }
}
