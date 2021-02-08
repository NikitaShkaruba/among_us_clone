using System.Collections.Generic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    public class Viewable : Shared.Game.PlayerLogic.Viewable
    {
        [SerializeField] private LayerMask targetMask;

        public List<ServerPlayer> visiblePlayers = new List<ServerPlayer>();

        public void FixedUpdate()
        {
            FindVisibleTargets();
        }

        private void FindVisibleTargets()
        {
            visiblePlayers.Clear();

            List<Vector2> viewPoints = ComputeViewPoints();

            foreach (Vector2 viewPoint in viewPoints)
            {
                Vector2 vectorsDifference = viewPoint - (Vector2)transform.position;

                RaycastHit2D[] raycastHit2Ds = Physics2D.RaycastAll(transform.position, vectorsDifference.normalized, vectorsDifference.magnitude, targetMask);
                foreach (RaycastHit2D raycastHit2D in raycastHit2Ds)
                {
                    ServerPlayer visibleServerPlayer = raycastHit2D.collider.GetComponentInParent<ServerPlayer>();

                    if (visibleServerPlayer.viewable == this)
                    {
                        continue;
                    }

                    if (visiblePlayers.Contains(visibleServerPlayer))
                    {
                        continue;
                    }

                    visiblePlayers.Add(visibleServerPlayer);
                }
            }
        }
    }
}
