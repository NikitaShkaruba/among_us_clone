using AmongUsClone.Client.Game.Interactions;
using UnityEngine;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI.InteractButtons
{
    [RequireComponent(typeof(Image))]
    public class LobbyInteractButton : MonoBehaviour
    {
        [SerializeField] private Interactor interactor;
        [SerializeField] private Image buttonImage;
        [SerializeField] private Sprite customizeButtonSprite;
        [SerializeField] private Sprite useButtonSprite;

        private void OnDestroy()
        {
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
            }
            else
            {
                buttonImage.overrideSprite = customizeButtonSprite;
            }
        }
    }
}
