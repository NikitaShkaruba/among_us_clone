using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.Interactions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI.InteractButtons
{
    [RequireComponent(typeof(Image))]
    public class LobbyInteractButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Interactor interactor;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite customizeButtonSprite;
        [SerializeField] private Sprite useButtonSprite;

        private void Start()
        {
            buttonImage.overrideSprite = useButtonSprite;
            buttonImage.color = Helpers.halfVisibleColor;
        }

        private void OnDestroy()
        {
            if (interactor == null)
            {
                return;
            }

            interactor.newInteractableChosen -= UpdateImage;
        }

        public void SetInteractor(Interactor interactor)
        {
            this.interactor = interactor;
            interactor.newInteractableChosen += UpdateImage;
        }

        private void UpdateImage(Interactable interactable)
        {
            if (interactable == null || interactable.type != InteractableType.Customize)
            {
                buttonImage.overrideSprite = useButtonSprite;
                buttonImage.color = Helpers.halfVisibleColor;
            }
            else
            {
                buttonImage.overrideSprite = customizeButtonSprite;
                buttonImage.color = Helpers.fullyVisibleColor;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactor.chosenInteractable == null)
            {
                return;
            }

            interactor.chosenInteractable.Interact();
        }
    }
}
