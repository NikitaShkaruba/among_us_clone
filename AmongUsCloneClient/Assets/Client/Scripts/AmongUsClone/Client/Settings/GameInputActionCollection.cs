// GENERATED AUTOMATICALLY FROM 'Assets/Client/Settings/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace AmongUsClone.Client.Settings
{
    public class @GameInputActionCollection : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @GameInputActionCollection()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""CrewmateControls"",
            ""id"": ""67f59c9b-fbc0-4402-8f0e-714fd0459e70"",
            ""actions"": [
                {
                    ""name"": ""MoveUp"",
                    ""type"": ""Button"",
                    ""id"": ""461ea9b6-8c3c-4859-ae55-beacacda19bb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveRight"",
                    ""type"": ""Button"",
                    ""id"": ""7a1ddad7-4c82-4801-9e35-31c7d0ac25a6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveDown"",
                    ""type"": ""Button"",
                    ""id"": ""96752462-e1dd-41ef-a76b-92884ffa1ded"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""e608f4ba-60eb-4394-a74b-c1db75466644"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""ef15cf7a-5a50-4672-91e1-8c8f071e7941"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleMiniMap"",
                    ""type"": ""Button"",
                    ""id"": ""d003d29c-05f5-4710-b682-37a9adaba164"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ToggleSettings"",
                    ""type"": ""Button"",
                    ""id"": ""f6b7672c-68e7-4a9b-8467-9ef4003c5d48"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""714b4a6b-015f-4a8c-84df-d948c3ce1366"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6baf14f7-9dd8-4b67-bd63-b57335151972"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleMiniMap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6bfed084-81be-448f-85ad-611b3ccde716"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveUp"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f23b7c15-c4e1-4ba1-8090-87309b870af5"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveDown"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f92c7ef3-2674-4012-8033-e97b98cedeec"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83fda8f2-9118-4da7-a016-501b6d3cf3b6"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f8de9a0f-8afc-465a-88d1-cbf43d965048"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ToggleSettings"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // CrewmateControls
            m_CrewmateControls = asset.FindActionMap("CrewmateControls", throwIfNotFound: true);
            m_CrewmateControls_MoveUp = m_CrewmateControls.FindAction("MoveUp", throwIfNotFound: true);
            m_CrewmateControls_MoveRight = m_CrewmateControls.FindAction("MoveRight", throwIfNotFound: true);
            m_CrewmateControls_MoveDown = m_CrewmateControls.FindAction("MoveDown", throwIfNotFound: true);
            m_CrewmateControls_MoveLeft = m_CrewmateControls.FindAction("MoveLeft", throwIfNotFound: true);
            m_CrewmateControls_Interact = m_CrewmateControls.FindAction("Interact", throwIfNotFound: true);
            m_CrewmateControls_ToggleMiniMap = m_CrewmateControls.FindAction("ToggleMiniMap", throwIfNotFound: true);
            m_CrewmateControls_ToggleSettings = m_CrewmateControls.FindAction("ToggleSettings", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        // CrewmateControls
        private readonly InputActionMap m_CrewmateControls;
        private ICrewmateControlsActions m_CrewmateControlsActionsCallbackInterface;
        private readonly InputAction m_CrewmateControls_MoveUp;
        private readonly InputAction m_CrewmateControls_MoveRight;
        private readonly InputAction m_CrewmateControls_MoveDown;
        private readonly InputAction m_CrewmateControls_MoveLeft;
        private readonly InputAction m_CrewmateControls_Interact;
        private readonly InputAction m_CrewmateControls_ToggleMiniMap;
        private readonly InputAction m_CrewmateControls_ToggleSettings;
        public struct CrewmateControlsActions
        {
            private @GameInputActionCollection m_Wrapper;
            public CrewmateControlsActions(@GameInputActionCollection wrapper) { m_Wrapper = wrapper; }
            public InputAction @MoveUp => m_Wrapper.m_CrewmateControls_MoveUp;
            public InputAction @MoveRight => m_Wrapper.m_CrewmateControls_MoveRight;
            public InputAction @MoveDown => m_Wrapper.m_CrewmateControls_MoveDown;
            public InputAction @MoveLeft => m_Wrapper.m_CrewmateControls_MoveLeft;
            public InputAction @Interact => m_Wrapper.m_CrewmateControls_Interact;
            public InputAction @ToggleMiniMap => m_Wrapper.m_CrewmateControls_ToggleMiniMap;
            public InputAction @ToggleSettings => m_Wrapper.m_CrewmateControls_ToggleSettings;
            public InputActionMap Get() { return m_Wrapper.m_CrewmateControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CrewmateControlsActions set) { return set.Get(); }
            public void SetCallbacks(ICrewmateControlsActions instance)
            {
                if (m_Wrapper.m_CrewmateControlsActionsCallbackInterface != null)
                {
                    @MoveUp.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveUp;
                    @MoveUp.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveUp;
                    @MoveUp.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveUp;
                    @MoveRight.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveRight;
                    @MoveRight.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveRight;
                    @MoveRight.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveRight;
                    @MoveDown.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveDown;
                    @MoveDown.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveDown;
                    @MoveDown.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveDown;
                    @MoveLeft.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveLeft;
                    @MoveLeft.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveLeft;
                    @MoveLeft.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMoveLeft;
                    @Interact.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnInteract;
                    @ToggleMiniMap.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleMiniMap;
                    @ToggleMiniMap.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleMiniMap;
                    @ToggleMiniMap.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleMiniMap;
                    @ToggleSettings.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleSettings;
                    @ToggleSettings.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleSettings;
                    @ToggleSettings.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleSettings;
                }
                m_Wrapper.m_CrewmateControlsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @MoveUp.started += instance.OnMoveUp;
                    @MoveUp.performed += instance.OnMoveUp;
                    @MoveUp.canceled += instance.OnMoveUp;
                    @MoveRight.started += instance.OnMoveRight;
                    @MoveRight.performed += instance.OnMoveRight;
                    @MoveRight.canceled += instance.OnMoveRight;
                    @MoveDown.started += instance.OnMoveDown;
                    @MoveDown.performed += instance.OnMoveDown;
                    @MoveDown.canceled += instance.OnMoveDown;
                    @MoveLeft.started += instance.OnMoveLeft;
                    @MoveLeft.performed += instance.OnMoveLeft;
                    @MoveLeft.canceled += instance.OnMoveLeft;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @ToggleMiniMap.started += instance.OnToggleMiniMap;
                    @ToggleMiniMap.performed += instance.OnToggleMiniMap;
                    @ToggleMiniMap.canceled += instance.OnToggleMiniMap;
                    @ToggleSettings.started += instance.OnToggleSettings;
                    @ToggleSettings.performed += instance.OnToggleSettings;
                    @ToggleSettings.canceled += instance.OnToggleSettings;
                }
            }
        }
        public CrewmateControlsActions @CrewmateControls => new CrewmateControlsActions(this);
        public interface ICrewmateControlsActions
        {
            void OnMoveUp(InputAction.CallbackContext context);
            void OnMoveRight(InputAction.CallbackContext context);
            void OnMoveDown(InputAction.CallbackContext context);
            void OnMoveLeft(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnToggleMiniMap(InputAction.CallbackContext context);
            void OnToggleSettings(InputAction.CallbackContext context);
        }
    }
}
