using AmongUsClone.Client.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    [RequireComponent(typeof(Camera))]
    public class PlayerCamera : MonoBehaviour
    {
        public GameObject target;

        [Range(0f, 0.99f)]
        public float smoothFactor = 0.8f;
        public Vector3 offset;

        public void Start()
        {
            transform.position = offset;
        }

        public void FixedUpdate()
        {
            if (target == null)
            {
                Logger.LogNotice(LoggerSection.PlayerCamera, "Not updating, because the target is null");
                return;
            }

            Vector3 desiredPosition = target.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1 - smoothFactor);
            transform.position = smoothedPosition;
        }
    }
}
