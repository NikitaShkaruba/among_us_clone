using AmongUsClone.Client.Logging;
using UnityEngine;
using Logger = AmongUsClone.Shared.Logging.Logger;
using Random = System.Random;

namespace AmongUsClone.Client.Game.PlayerLogic
{
    [RequireComponent(typeof(Camera))]
    public class PlayerCamera : MonoBehaviour
    {
        public GameObject target;

        [Range(0f, 0.99f)]
        public float smoothFactor = 0.8f;

        public Vector3 offset;

        [Range(0f, 0.99f)]
        public float shakeFactor = 0.005f;
        public bool shaking;

        public void Start()
        {
            transform.position = offset;
        }

        public void LateUpdate()
        {
            if (target == null)
            {
                Logger.LogNotice(LoggerSection.PlayerCamera, "Not updating, because the target is null");
                return;
            }

            Vector3 desiredPosition = target.transform.position + offset;
            if (shaking)
            {
                Random random = new Random();
                int randomX = random.Next(-50, 50);
                int randomY = random.Next(-50, 50);
                desiredPosition += new Vector3(randomX * shakeFactor, randomY * shakeFactor, 0);
            }

            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, 1 - smoothFactor);
            transform.position = smoothedPosition;
        }
    }
}
