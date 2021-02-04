using System.Collections.Generic;
using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public abstract class Viewable : MonoBehaviour
    {
        public const float ViewAngle = 360;

        [SerializeField] protected LayerMask obstacleMask;
        [SerializeField] protected float meshResolution;
        public float viewRadius;

        protected List<Vector2> ComputeViewPoints()
        {
            int raysAmount = Mathf.RoundToInt(ViewAngle * meshResolution);
            float raySize = ViewAngle / raysAmount;

            List<Vector2> viewPoints = new List<Vector2>();

            for (int rayIndex = 0; rayIndex < raysAmount; rayIndex++)
            {
                float angle = transform.eulerAngles.y - ViewAngle / 2 + raySize * rayIndex;
                Vector2 viewPoint = ViewCast(angle);

                viewPoints.Add(viewPoint);
            }

            return viewPoints;
        }

        private Vector2 ViewCast(float angle)
        {
            Vector2 direction = ComputeDirectionByAngle(angle);

            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, direction, viewRadius, obstacleMask);
            if (!raycastHit2D)
            {
                return (Vector2) transform.position + direction * viewRadius; // Return max viewable point
            }

            return raycastHit2D.point;
        }

        private static Vector2 ComputeDirectionByAngle(float angle)
        {
            float x = Mathf.Sin(angle * Mathf.Deg2Rad);
            float y = Mathf.Cos(angle * Mathf.Deg2Rad);

            return new Vector2(x, y);
        }
    }
}
