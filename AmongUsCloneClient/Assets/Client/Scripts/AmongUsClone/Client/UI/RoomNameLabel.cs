using System.Collections;
using AmongUsClone.Client.Game;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI
{
    [RequireComponent(typeof(Text))]
    public class RoomNameLabel : MonoBehaviour
    {
        public Text label;
        public float transitionSpeed;
        public float hideDepth;

        private Vector3 initialPosition;
        private bool isTransitionDirectionUp;
        private bool transitionGoing;

        private void Awake()
        {
            initialPosition = transform.position;

            Room[] rooms = FindObjectsOfType<Room>();

            foreach (Room room in rooms)
            {
                room.onControlledPlayerEnter += OnControlledPlayerEnter;
                room.onControlledPlayerExit += OnControlledPlayerExit;
            }
        }

        private void OnDestroy()
        {
            Room[] rooms = FindObjectsOfType<Room>();

            foreach (Room room in rooms)
            {
                room.onControlledPlayerEnter -= OnControlledPlayerEnter;
                room.onControlledPlayerExit -= OnControlledPlayerExit;
            }
        }

        private void OnControlledPlayerEnter(string roomName)
        {
            label.text = roomName;
            isTransitionDirectionUp = true;

            if (!transitionGoing)
            {
                transitionGoing = true;
                StartCoroutine(UpdateLabelPosition());
            }
        }

        private void OnControlledPlayerExit()
        {
            isTransitionDirectionUp = false;

            if (!transitionGoing)
            {
                transitionGoing = true;
                StartCoroutine(UpdateLabelPosition());
            }
        }

        private IEnumerator UpdateLabelPosition()
        {
            yield return new WaitForSeconds(0.01f);

            float nextY = isTransitionDirectionUp ? transitionSpeed : -transitionSpeed;
            label.transform.localPosition += new Vector3(0f, nextY, 0f);

            bool isTransitionFinished = isTransitionDirectionUp ? label.transform.position.y >= initialPosition.y : initialPosition.y - label.transform.position.y >= hideDepth;
            if (isTransitionFinished)
            {
                transitionGoing = false;
            }
            else
            {
                StartCoroutine(UpdateLabelPosition());
            }
        }
    }
}
