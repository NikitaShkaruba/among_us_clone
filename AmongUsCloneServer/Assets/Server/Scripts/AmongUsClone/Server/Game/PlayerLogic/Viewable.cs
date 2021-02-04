using System.Collections.Generic;
using UnityEngine;

namespace AmongUsClone.Server.Game.PlayerLogic
{
    public class Viewable : Shared.Game.PlayerLogic.Viewable
    {
        [SerializeField] private LayerMask targetMask;

        public List<Player> visiblePlayers = new List<Player>();

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
                    Player visiblePlayer = raycastHit2D.collider.GetComponentInParent<Player>();

                    if (visiblePlayer.viewable == this)
                    {
                        continue;
                    }

                    if (visiblePlayers.Contains(visiblePlayer))
                    {
                        continue;
                    }

                    visiblePlayers.Add(visiblePlayer);
                }
            }
        }
    }
}
