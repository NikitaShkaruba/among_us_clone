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
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""d69586b1-0e3c-4efa-b777-67e8dab5e313"",
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
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""e3b2418e-77a2-4516-bdae-8f9731b1c1c0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""6e619de1-ace2-437a-aedd-aaa3ac1d4bb7"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""ae24de43-d91e-4e7a-842d-c2405c9893f6"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""4592b9c5-bb68-4aa3-b751-46e24abf4f74"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""50897eed-d393-4429-a9ec-998655d2b08e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
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
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // CrewmateControls
            m_CrewmateControls = asset.FindActionMap("CrewmateControls", throwIfNotFound: true);
            m_CrewmateControls_Move = m_CrewmateControls.FindAction("Move", throwIfNotFound: true);
            m_CrewmateControls_Interact = m_CrewmateControls.FindAction("Interact", throwIfNotFound: true);
            m_CrewmateControls_ToggleMiniMap = m_CrewmateControls.FindAction("ToggleMiniMap", throwIfNotFound: true);
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
        private readonly InputAction m_CrewmateControls_Move;
        private readonly InputAction m_CrewmateControls_Interact;
        private readonly InputAction m_CrewmateControls_ToggleMiniMap;
        public struct CrewmateControlsActions
        {
            private @GameInputActionCollection m_Wrapper;
            public CrewmateControlsActions(@GameInputActionCollection wrapper) { m_Wrapper = wrapper; }
            public InputAction @Move => m_Wrapper.m_CrewmateControls_Move;
            public InputAction @Interact => m_Wrapper.m_CrewmateControls_Interact;
            public InputAction @ToggleMiniMap => m_Wrapper.m_CrewmateControls_ToggleMiniMap;
            public InputActionMap Get() { return m_Wrapper.m_CrewmateControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CrewmateControlsActions set) { return set.Get(); }
            public void SetCallbacks(ICrewmateControlsActions instance)
            {
                if (m_Wrapper.m_CrewmateControlsActionsCallbackInterface != null)
                {
                    @Move.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMove;
                    @Move.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMove;
                    @Move.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnMove;
                    @Interact.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnInteract;
                    @Interact.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnInteract;
                    @Interact.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnInteract;
                    @ToggleMiniMap.started -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleMiniMap;
                    @ToggleMiniMap.performed -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleMiniMap;
                    @ToggleMiniMap.canceled -= m_Wrapper.m_CrewmateControlsActionsCallbackInterface.OnToggleMiniMap;
                }
                m_Wrapper.m_CrewmateControlsActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Move.started += instance.OnMove;
                    @Move.performed += instance.OnMove;
                    @Move.canceled += instance.OnMove;
                    @Interact.started += instance.OnInteract;
                    @Interact.performed += instance.OnInteract;
                    @Interact.canceled += instance.OnInteract;
                    @ToggleMiniMap.started += instance.OnToggleMiniMap;
                    @ToggleMiniMap.performed += instance.OnToggleMiniMap;
                    @ToggleMiniMap.canceled += instance.OnToggleMiniMap;
                }
            }
        }
        public CrewmateControlsActions @CrewmateControls => new CrewmateControlsActions(this);
        public interface ICrewmateControlsActions
        {
            void OnMove(InputAction.CallbackContext context);
            void OnInteract(InputAction.CallbackContext context);
            void OnToggleMiniMap(InputAction.CallbackContext context);
        }
    }
}
