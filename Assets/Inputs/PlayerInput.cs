//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Inputs/PlayerInput.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInput : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""CharacterControls"",
            ""id"": ""4594cb4a-1103-4bfd-be74-55e947ee51fe"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""1c7c4041-3a1b-4751-9039-30e8f2c490e5"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": ""NormalizeVector2"",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""529e613b-cb31-47e0-b761-b49604a38514"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold(duration=0.075)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""InteractRopePlacement"",
                    ""type"": ""Button"",
                    ""id"": ""18739fb6-25d0-4eae-8709-44c3cf994318"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""EnterAim"",
                    ""type"": ""Button"",
                    ""id"": ""ae0d69ac-f90a-45b3-9830-3ee1788f8153"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ExitAim"",
                    ""type"": ""Button"",
                    ""id"": ""32d52196-dd77-45eb-8a59-59e0aad86eaf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ConfirmRopePlacement"",
                    ""type"": ""Button"",
                    ""id"": ""887b3b68-e2c9-4541-9c36-4c3a704a7157"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DetachLastRopePlacement"",
                    ""type"": ""Button"",
                    ""id"": ""756afd89-f100-4df4-884a-3626a5b41777"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""DetachFirstRopePlacement"",
                    ""type"": ""Button"",
                    ""id"": ""66cb60df-315a-4acb-bcf4-a07f92cd1efb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectObject"",
                    ""type"": ""Button"",
                    ""id"": ""571d5e3b-fda9-4cdd-a078-16c8f30cd928"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SelectSurface"",
                    ""type"": ""Button"",
                    ""id"": ""a763b14b-8b1e-43fa-9080-7b9a69b25041"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""5f03c9c4-3890-4e88-9ec6-2bf07df62938"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""39eb56ae-0959-4d4e-8adf-46d70343d812"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""357b313d-b449-4cb9-ad8f-c186644e45ac"",
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
                    ""id"": ""dc8053f6-5f9b-4268-8f8b-39ca9e030f7f"",
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
                    ""id"": ""58d73d30-bdac-4cdf-a1a1-47099d377050"",
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
                    ""id"": ""b1d31b97-a02d-4786-b787-fa110960020d"",
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
                    ""id"": ""11ebaf3f-1ed0-4f39-a719-15ed5120574e"",
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
                    ""id"": ""5041a377-b4d1-49fc-bc47-2e314311e3cc"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""336030d3-92fb-45c5-aeda-3728f8ee2384"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""418096dc-a775-4a0e-9f5f-4a0889a8a579"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InteractRopePlacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""607aedff-b16d-488d-bed1-b1323e2c2e79"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""EnterAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5d791c52-b72d-4f4e-90a8-25b2d335e2c7"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ConfirmRopePlacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""253db201-1430-48d4-8edc-fc9a018ccaa9"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DetachLastRopePlacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""77789578-8ab6-4fae-acd9-6429bca05e79"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""DetachFirstRopePlacement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1b015cac-1e65-40d9-b7fd-3ba8d707893c"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExitAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfb47b10-83bc-4599-a5c3-61397e9e36f6"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ExitAim"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a43b7d77-bced-4db5-85f7-9b9b400805ed"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectObject"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a510a224-99e2-40ce-91eb-ac89487c2028"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SelectSurface"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15a35f93-e904-4c0f-8f1a-09cb9c442ceb"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""OtherInteraction"",
            ""id"": ""2c298066-2be8-4843-b951-8c0843e0380e"",
            ""actions"": [
                {
                    ""name"": ""SetRespawnPoint"",
                    ""type"": ""Button"",
                    ""id"": ""f3f8420b-972b-4219-b91b-afc18c513df1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""093e8ab0-9b11-402a-b9ea-0ffd5850d6a2"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SetRespawnPoint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // CharacterControls
        m_CharacterControls = asset.FindActionMap("CharacterControls", throwIfNotFound: true);
        m_CharacterControls_Move = m_CharacterControls.FindAction("Move", throwIfNotFound: true);
        m_CharacterControls_Jump = m_CharacterControls.FindAction("Jump", throwIfNotFound: true);
        m_CharacterControls_InteractRopePlacement = m_CharacterControls.FindAction("InteractRopePlacement", throwIfNotFound: true);
        m_CharacterControls_EnterAim = m_CharacterControls.FindAction("EnterAim", throwIfNotFound: true);
        m_CharacterControls_ExitAim = m_CharacterControls.FindAction("ExitAim", throwIfNotFound: true);
        m_CharacterControls_ConfirmRopePlacement = m_CharacterControls.FindAction("ConfirmRopePlacement", throwIfNotFound: true);
        m_CharacterControls_DetachLastRopePlacement = m_CharacterControls.FindAction("DetachLastRopePlacement", throwIfNotFound: true);
        m_CharacterControls_DetachFirstRopePlacement = m_CharacterControls.FindAction("DetachFirstRopePlacement", throwIfNotFound: true);
        m_CharacterControls_SelectObject = m_CharacterControls.FindAction("SelectObject", throwIfNotFound: true);
        m_CharacterControls_SelectSurface = m_CharacterControls.FindAction("SelectSurface", throwIfNotFound: true);
        m_CharacterControls_Crouch = m_CharacterControls.FindAction("Crouch", throwIfNotFound: true);
        // OtherInteraction
        m_OtherInteraction = asset.FindActionMap("OtherInteraction", throwIfNotFound: true);
        m_OtherInteraction_SetRespawnPoint = m_OtherInteraction.FindAction("SetRespawnPoint", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // CharacterControls
    private readonly InputActionMap m_CharacterControls;
    private ICharacterControlsActions m_CharacterControlsActionsCallbackInterface;
    private readonly InputAction m_CharacterControls_Move;
    private readonly InputAction m_CharacterControls_Jump;
    private readonly InputAction m_CharacterControls_InteractRopePlacement;
    private readonly InputAction m_CharacterControls_EnterAim;
    private readonly InputAction m_CharacterControls_ExitAim;
    private readonly InputAction m_CharacterControls_ConfirmRopePlacement;
    private readonly InputAction m_CharacterControls_DetachLastRopePlacement;
    private readonly InputAction m_CharacterControls_DetachFirstRopePlacement;
    private readonly InputAction m_CharacterControls_SelectObject;
    private readonly InputAction m_CharacterControls_SelectSurface;
    private readonly InputAction m_CharacterControls_Crouch;
    public struct CharacterControlsActions
    {
        private @PlayerInput m_Wrapper;
        public CharacterControlsActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_CharacterControls_Move;
        public InputAction @Jump => m_Wrapper.m_CharacterControls_Jump;
        public InputAction @InteractRopePlacement => m_Wrapper.m_CharacterControls_InteractRopePlacement;
        public InputAction @EnterAim => m_Wrapper.m_CharacterControls_EnterAim;
        public InputAction @ExitAim => m_Wrapper.m_CharacterControls_ExitAim;
        public InputAction @ConfirmRopePlacement => m_Wrapper.m_CharacterControls_ConfirmRopePlacement;
        public InputAction @DetachLastRopePlacement => m_Wrapper.m_CharacterControls_DetachLastRopePlacement;
        public InputAction @DetachFirstRopePlacement => m_Wrapper.m_CharacterControls_DetachFirstRopePlacement;
        public InputAction @SelectObject => m_Wrapper.m_CharacterControls_SelectObject;
        public InputAction @SelectSurface => m_Wrapper.m_CharacterControls_SelectSurface;
        public InputAction @Crouch => m_Wrapper.m_CharacterControls_Crouch;
        public InputActionMap Get() { return m_Wrapper.m_CharacterControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CharacterControlsActions set) { return set.Get(); }
        public void SetCallbacks(ICharacterControlsActions instance)
        {
            if (m_Wrapper.m_CharacterControlsActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnJump;
                @InteractRopePlacement.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnInteractRopePlacement;
                @InteractRopePlacement.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnInteractRopePlacement;
                @InteractRopePlacement.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnInteractRopePlacement;
                @EnterAim.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnEnterAim;
                @EnterAim.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnEnterAim;
                @EnterAim.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnEnterAim;
                @ExitAim.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnExitAim;
                @ExitAim.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnExitAim;
                @ExitAim.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnExitAim;
                @ConfirmRopePlacement.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnConfirmRopePlacement;
                @ConfirmRopePlacement.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnConfirmRopePlacement;
                @ConfirmRopePlacement.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnConfirmRopePlacement;
                @DetachLastRopePlacement.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnDetachLastRopePlacement;
                @DetachLastRopePlacement.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnDetachLastRopePlacement;
                @DetachLastRopePlacement.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnDetachLastRopePlacement;
                @DetachFirstRopePlacement.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnDetachFirstRopePlacement;
                @DetachFirstRopePlacement.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnDetachFirstRopePlacement;
                @DetachFirstRopePlacement.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnDetachFirstRopePlacement;
                @SelectObject.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnSelectObject;
                @SelectObject.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnSelectObject;
                @SelectObject.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnSelectObject;
                @SelectSurface.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnSelectSurface;
                @SelectSurface.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnSelectSurface;
                @SelectSurface.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnSelectSurface;
                @Crouch.started -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnCrouch;
                @Crouch.performed -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnCrouch;
                @Crouch.canceled -= m_Wrapper.m_CharacterControlsActionsCallbackInterface.OnCrouch;
            }
            m_Wrapper.m_CharacterControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @InteractRopePlacement.started += instance.OnInteractRopePlacement;
                @InteractRopePlacement.performed += instance.OnInteractRopePlacement;
                @InteractRopePlacement.canceled += instance.OnInteractRopePlacement;
                @EnterAim.started += instance.OnEnterAim;
                @EnterAim.performed += instance.OnEnterAim;
                @EnterAim.canceled += instance.OnEnterAim;
                @ExitAim.started += instance.OnExitAim;
                @ExitAim.performed += instance.OnExitAim;
                @ExitAim.canceled += instance.OnExitAim;
                @ConfirmRopePlacement.started += instance.OnConfirmRopePlacement;
                @ConfirmRopePlacement.performed += instance.OnConfirmRopePlacement;
                @ConfirmRopePlacement.canceled += instance.OnConfirmRopePlacement;
                @DetachLastRopePlacement.started += instance.OnDetachLastRopePlacement;
                @DetachLastRopePlacement.performed += instance.OnDetachLastRopePlacement;
                @DetachLastRopePlacement.canceled += instance.OnDetachLastRopePlacement;
                @DetachFirstRopePlacement.started += instance.OnDetachFirstRopePlacement;
                @DetachFirstRopePlacement.performed += instance.OnDetachFirstRopePlacement;
                @DetachFirstRopePlacement.canceled += instance.OnDetachFirstRopePlacement;
                @SelectObject.started += instance.OnSelectObject;
                @SelectObject.performed += instance.OnSelectObject;
                @SelectObject.canceled += instance.OnSelectObject;
                @SelectSurface.started += instance.OnSelectSurface;
                @SelectSurface.performed += instance.OnSelectSurface;
                @SelectSurface.canceled += instance.OnSelectSurface;
                @Crouch.started += instance.OnCrouch;
                @Crouch.performed += instance.OnCrouch;
                @Crouch.canceled += instance.OnCrouch;
            }
        }
    }
    public CharacterControlsActions @CharacterControls => new CharacterControlsActions(this);

    // OtherInteraction
    private readonly InputActionMap m_OtherInteraction;
    private IOtherInteractionActions m_OtherInteractionActionsCallbackInterface;
    private readonly InputAction m_OtherInteraction_SetRespawnPoint;
    public struct OtherInteractionActions
    {
        private @PlayerInput m_Wrapper;
        public OtherInteractionActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @SetRespawnPoint => m_Wrapper.m_OtherInteraction_SetRespawnPoint;
        public InputActionMap Get() { return m_Wrapper.m_OtherInteraction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OtherInteractionActions set) { return set.Get(); }
        public void SetCallbacks(IOtherInteractionActions instance)
        {
            if (m_Wrapper.m_OtherInteractionActionsCallbackInterface != null)
            {
                @SetRespawnPoint.started -= m_Wrapper.m_OtherInteractionActionsCallbackInterface.OnSetRespawnPoint;
                @SetRespawnPoint.performed -= m_Wrapper.m_OtherInteractionActionsCallbackInterface.OnSetRespawnPoint;
                @SetRespawnPoint.canceled -= m_Wrapper.m_OtherInteractionActionsCallbackInterface.OnSetRespawnPoint;
            }
            m_Wrapper.m_OtherInteractionActionsCallbackInterface = instance;
            if (instance != null)
            {
                @SetRespawnPoint.started += instance.OnSetRespawnPoint;
                @SetRespawnPoint.performed += instance.OnSetRespawnPoint;
                @SetRespawnPoint.canceled += instance.OnSetRespawnPoint;
            }
        }
    }
    public OtherInteractionActions @OtherInteraction => new OtherInteractionActions(this);
    public interface ICharacterControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnInteractRopePlacement(InputAction.CallbackContext context);
        void OnEnterAim(InputAction.CallbackContext context);
        void OnExitAim(InputAction.CallbackContext context);
        void OnConfirmRopePlacement(InputAction.CallbackContext context);
        void OnDetachLastRopePlacement(InputAction.CallbackContext context);
        void OnDetachFirstRopePlacement(InputAction.CallbackContext context);
        void OnSelectObject(InputAction.CallbackContext context);
        void OnSelectSurface(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
    }
    public interface IOtherInteractionActions
    {
        void OnSetRespawnPoint(InputAction.CallbackContext context);
    }
}
