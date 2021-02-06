using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace AmongUsClone.Shared.Game.PlayerLogic
{
    public class NearbyMonoBehavioursChooser<T> : MonoBehaviour where T : MonoBehaviour
    {
        public float maxDistance;
        protected List<T> chosables;

        [CanBeNull] public T chosen { get; private set; }

        protected void Start()
        {
            CacheChosables();
        }

        protected void Update()
        {
            chosen = FindClosestInRange();
        }

        public void CacheChosables()
        {
            chosables = FindObjectsOfType<T>().ToList();
        }

        [CanBeNull]
        private T FindClosestInRange()
        {
            float distanceToClosest = float.PositiveInfinity;
            T closest = null;

            foreach (T chosable in chosables)
            {
                Vector3 positionDifferenceWithControlledPlayer = transform.position - chosable.transform.position;
                if (positionDifferenceWithControlledPlayer.magnitude > maxDistance || positionDifferenceWithControlledPlayer.magnitude > distanceToClosest)
                {
                    continue;
                }

                distanceToClosest = positionDifferenceWithControlledPlayer.magnitude;
                closest = chosable;
            }

            return closest;
        }
    }
}
