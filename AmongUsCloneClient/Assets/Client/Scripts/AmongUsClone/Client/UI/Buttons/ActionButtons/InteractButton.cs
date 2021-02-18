using AmongUsClone.Client.Game;
using AmongUsClone.Client.Game.GamePhaseManagers;
using AmongUsClone.Client.Game.Interactions;
using AmongUsClone.Client.Game.Maps;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AmongUsClone.Client.UI.Buttons.ActionButtons
{
    [RequireComponent(typeof(Image))]
    public class InteractButton : MonoBehaviour, IPointerClickHandler
    {
        [Header("Scriptable objects")]
        [SerializeField] private LobbyGamePhase lobbyGamePhase;
        [SerializeField] private PlayGamePhase playGamePhase;
        [SerializeField] private PlayersManager playersManager;

        [Header("General")]
        [SerializeField] private Interactor interactor;
        [SerializeField] private Image buttonImage;

        [Header("Button sprites")]
        [SerializeField] private Sprite customizeButtonSprite;
        [SerializeField] private Sprite useAdminPanelButtonSprite;
        [SerializeField] private Sprite useSecurityPanelButtonSprite;
        [SerializeField] private Sprite useButtonSprite;

        private bool isDisabled;
        private bool isHidden;

        private void Start()
        {
            buttonImage.overrideSprite = useButtonSprite;
            buttonImage.color = Helpers.halfVisibleColor;
        }

        private void OnEnable()
        {
            SetupCallbacks();
        }

        private void OnDisable()
        {
            RemoveCallbacks();
        }

        public void SetInteractor(Interactor interactor)
        {
            this.interactor = interactor;
            interactor.newInteractableChosen += UpdateState;
            UpdateState();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactor.chosen == null || isHidden || isDisabled)
            {
                return;
            }

            playersManager.controlledClientPlayer.clientControllable.OnInteract();
        }

        public void UpdateCallbacks()
        {
            RemoveCallbacks();
            SetupCallbacks();
        }

        private void UpdateState()
        {
            isDisabled = false;
            isHidden = false;

            if (lobbyGamePhase.lobby != null)
            {
                isDisabled = lobbyGamePhase.lobby.activeSceneUserInterface.settingsButton.SettingsMenuActive;
                isHidden = false;
            }

            if (playGamePhase.clientSkeld != null)
            {
                ClientSkeld clientSkeld = playGamePhase.clientSkeld;

                isDisabled = clientSkeld.playGamePhaseUserInterface.activeSceneUserInterface.settingsButton.SettingsMenuActive;
                isHidden = clientSkeld.playGamePhaseUserInterface.minimapButton.IsMinimapShown ||
                           clientSkeld.adminPanel.isControlledPlayerViewing ||
                           clientSkeld.securityPanel.isControlledPlayerViewing;
            }

            UpdateImage(interactor.chosen);
        }

        private void UpdateImage(Interactable interactable)
        {
            if (isHidden && buttonImage.enabled)
            {
                buttonImage.enabled = false;
            }
            else if (!isHidden && !buttonImage.enabled)
            {
                buttonImage.enabled = true;
            }

            if (interactable == null || isDisabled)
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

                case InteractableType.UseSecurityPanel:
                    buttonImage.overrideSprite = useSecurityPanelButtonSprite;
                    buttonImage.color = Helpers.fullyVisibleColor;
                    break;

                default:
                    buttonImage.overrideSprite = useButtonSprite;
                    buttonImage.color = Helpers.halfVisibleColor;
                    break;
            }
        }

        private void SetupCallbacks()
        {
            if (interactor != null)
            {
                interactor.newInteractableChosen += UpdateState;
            }

            if (lobbyGamePhase.lobby != null)
            {
                ActiveSceneUserInterface lobbyUserInterface = lobbyGamePhase.lobby.activeSceneUserInterface;
                lobbyUserInterface.settingsButton.onToggle += UpdateState;
            }

            if (playGamePhase.clientSkeld != null)
            {
                PlayGamePhaseUserInterface skeldUserInterface = playGamePhase.clientSkeld.playGamePhaseUserInterface;
                skeldUserInterface.minimapButton.onToggle += UpdateState;
                skeldUserInterface.activeSceneUserInterface.settingsButton.onToggle += UpdateState;

                ClientSkeld clientSkeld = playGamePhase.clientSkeld;
                clientSkeld.adminPanel.onInteraction += UpdateState;
                clientSkeld.securityPanel.onInterfaceToggle += UpdateState;
            }
        }

        private void RemoveCallbacks()
        {
            if (interactor != null)
            {
                interactor.newInteractableChosen -= UpdateState;
            }

            if (lobbyGamePhase.lobby != null)
            {
                ActiveSceneUserInterface lobbyUserInterface = lobbyGamePhase.lobby.activeSceneUserInterface;
                lobbyUserInterface.settingsButton.onToggle -= UpdateState;
            }

            if (playGamePhase.clientSkeld != null)
            {
                PlayGamePhaseUserInterface skeldUserInterface = playGamePhase.clientSkeld.playGamePhaseUserInterface;
                skeldUserInterface.minimapButton.onToggle -= UpdateState;
                skeldUserInterface.activeSceneUserInterface.settingsButton.onToggle -= UpdateState;

                ClientSkeld clientSkeld = playGamePhase.clientSkeld;
                clientSkeld.adminPanel.onInteraction -= UpdateState;
                clientSkeld.securityPanel.onInterfaceToggle -= UpdateState;
            }
        }
    }
}
