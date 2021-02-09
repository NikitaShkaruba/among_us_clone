using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.Interactions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI.Buttons.ActionButtons
{
    [RequireComponent(typeof(Image))]
    public class InteractButton : MonoBehaviour, IPointerClickHandler
    {
        [Header("General")]
        [SerializeField] private Interactor interactor;
        [SerializeField] private Image buttonImage;
        [SerializeField] private SettingsButton settingsButton;

        [Header("Button sprites")]
        [SerializeField] private Sprite customizeButtonSprite;
        [SerializeField] private Sprite useAdminPanelButtonSprite;
        [SerializeField] private Sprite useButtonSprite;

        private void Start()
        {
            buttonImage.overrideSprite = useButtonSprite;
            buttonImage.color = Helpers.halfVisibleColor;
        }

        private void OnEnable()
        {
            settingsButton.onSettingsToggle += UpdateImageAfterSettingsUpdate;
        }

        private void OnDisable()
        {
            settingsButton.onSettingsToggle -= UpdateImageAfterSettingsUpdate;
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
            if (interactable == null || settingsButton.SettingsMenuActive)
            {
                buttonImage.overrideSprite = useButtonSprite;
                buttonImage.color = Helpers.halfVisibleColor;
                return;
            }

            switch (interactable.type)
            {
                case InteractableType.Customize:
                    buttonImage.overrideSprite = customizeButtonSprite;
                    buttonImage.color = Helpers.fullyVisibleColor;
                    break;

                case InteractableType.UseAdminPanel:
                    buttonImage.overrideSprite = useAdminPanelButtonSprite;
                    buttonImage.color = Helpers.fullyVisibleColor;
                    break;

                default:
                    buttonImage.overrideSprite = useButtonSprite;
                    buttonImage.color = Helpers.halfVisibleColor;
                    break;
            }
        }

        private void UpdateImageAfterSettingsUpdate()
        {
            UpdateImage(interactor.chosen);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactor.chosen == null || settingsButton.SettingsMenuActive)
            {
                return;
            }

            interactor.chosen.Interact();
        }
    }
}
