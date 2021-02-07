using System.Collections;
using AmongUsClone.Client.Game;
using AmongUsClone.Shared.Game.Rooms;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI
{
    [RequireComponent(typeof(Text))]
    public class RoomNameLabel : MonoBehaviour
    {
        [SerializeField] private PlayersManager playersManager;

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
                room.walkingPlayersDetectable.SubscribeToPlayerEvents(playersManager.controlledPlayer.information.id, OnControlledPlayerEnter, OnControlledPlayerExit);
            }
        }

        private void OnDestroy()
        {
            Room[] rooms = FindObjectsOfType<Room>();

            foreach (Room room in rooms)
            {
                room.walkingPlayersDetectable.UnsubscribeFromPlayerEvents(playersManager.controlledPlayer.information.id, OnControlledPlayerEnter, OnControlledPlayerExit);
            }
        }

        private void OnControlledPlayerEnter(Room room)
        {
            label.text = room.name;
            isTransitionDirectionUp = true;

            if (!transitionGoing)
            {
                transitionGoing = true;
                StartCoroutine(UpdateLabelPosition());
            }
        }

        private void OnControlledPlayerExit(Room room)
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
