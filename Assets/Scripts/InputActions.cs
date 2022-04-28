// GENERATED AUTOMATICALLY FROM 'Assets/InputSettings.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Scifi.Input
{
    public class @InputActions : IInputActionCollection, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputActions()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSettings"",
    ""maps"": [
        {
            ""name"": ""carControl"",
            ""id"": ""e5ea01be-bb39-4e84-8a2c-501f4bca82f1"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Button"",
                    ""id"": ""eccd7770-6b27-4ce7-ba7c-701f2f6faeea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""PassThrough"",
                    ""id"": ""15500712-f1a9-4740-9ade-73b60e613f2d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Lift"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8d572be5-4ba0-4c6f-ad22-2e0de4018264"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""8447fd7d-900e-4353-9b8d-3047be22dd65"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""81deb341-2e30-47e1-a345-94e8484ecb88"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""7578ff8a-2bf0-423c-97b8-015f3c6c53ce"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""77627869-180c-46da-9148-815ffb076215"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""2537fb43-df17-40e3-8186-37bbc304fda1"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Arrows"",
                    ""id"": ""92e672cc-beb9-4478-ba6d-4d57622ef947"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0a88b26e-ec55-44aa-a230-9a81537c5680"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""c275d630-f011-43fe-933a-a9b099763c70"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d94e1441-2d0c-4330-ae79-b3e39dd0f6e6"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d2ee937e-8a63-4429-a126-d067194c768e"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""a784edc1-05e7-4a94-807b-bd2ced7b4108"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Buttons"",
                    ""id"": ""88a30c77-dea9-4b57-8b6a-7fbba32fce77"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""848aff29-a2da-4f94-84ad-2c6bb027e9ca"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""2a07b864-8372-43ad-baaf-a87512e74326"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Functions"",
                    ""id"": ""f9688cb2-8999-41c5-8277-4bd32efc95af"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""30741c7c-b15a-4fc9-882a-bb322082b37c"",
                    ""path"": ""<Keyboard>/rightCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""b9607c42-3dcb-4970-b2c5-9011f6b9928e"",
                    ""path"": ""<Keyboard>/rightShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Lift"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
            // carControl
            m_carControl = asset.FindActionMap("carControl", throwIfNotFound: true);
            m_carControl_Movement = m_carControl.FindAction("Movement", throwIfNotFound: true);
            m_carControl_Look = m_carControl.FindAction("Look", throwIfNotFound: true);
            m_carControl_Lift = m_carControl.FindAction("Lift", throwIfNotFound: true);
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

        // carControl
        private readonly InputActionMap m_carControl;
        private ICarControlActions m_CarControlActionsCallbackInterface;
        private readonly InputAction m_carControl_Movement;
        private readonly InputAction m_carControl_Look;
        private readonly InputAction m_carControl_Lift;
        public struct CarControlActions
        {
            private @InputActions m_Wrapper;
            public CarControlActions(@InputActions wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_carControl_Movement;
            public InputAction @Look => m_Wrapper.m_carControl_Look;
            public InputAction @Lift => m_Wrapper.m_carControl_Lift;
            public InputActionMap Get() { return m_Wrapper.m_carControl; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(CarControlActions set) { return set.Get(); }
            public void SetCallbacks(ICarControlActions instance)
            {
                if (m_Wrapper.m_CarControlActionsCallbackInterface != null)
                {
                    @Movement.started -= m_Wrapper.m_CarControlActionsCallbackInterface.OnMovement;
                    @Movement.performed -= m_Wrapper.m_CarControlActionsCallbackInterface.OnMovement;
                    @Movement.canceled -= m_Wrapper.m_CarControlActionsCallbackInterface.OnMovement;
                    @Look.started -= m_Wrapper.m_CarControlActionsCallbackInterface.OnLook;
                    @Look.performed -= m_Wrapper.m_CarControlActionsCallbackInterface.OnLook;
                    @Look.canceled -= m_Wrapper.m_CarControlActionsCallbackInterface.OnLook;
                    @Lift.started -= m_Wrapper.m_CarControlActionsCallbackInterface.OnLift;
                    @Lift.performed -= m_Wrapper.m_CarControlActionsCallbackInterface.OnLift;
                    @Lift.canceled -= m_Wrapper.m_CarControlActionsCallbackInterface.OnLift;
                }
                m_Wrapper.m_CarControlActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Movement.started += instance.OnMovement;
                    @Movement.performed += instance.OnMovement;
                    @Movement.canceled += instance.OnMovement;
                    @Look.started += instance.OnLook;
                    @Look.performed += instance.OnLook;
                    @Look.canceled += instance.OnLook;
                    @Lift.started += instance.OnLift;
                    @Lift.performed += instance.OnLift;
                    @Lift.canceled += instance.OnLift;
                }
            }
        }
        public CarControlActions @carControl => new CarControlActions(this);
        public interface ICarControlActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnLook(InputAction.CallbackContext context);
            void OnLift(InputAction.CallbackContext context);
        }
    }
}
