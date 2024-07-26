//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.6.3
//     from Assets/InputAssets/PlayerInputConfig.inputactions
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

public partial class @PlayerInputConfig: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputConfig()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputConfig"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""de8bb083-b733-46f7-903f-fed32a513444"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""8957475d-5fe3-49a7-839e-3a8716136c83"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""a370d4a6-95ea-418e-bf91-33134f35ab91"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot_Left"",
                    ""type"": ""Value"",
                    ""id"": ""081671cd-e5da-4231-8eb1-feabfa4d30b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Shoot_Right"",
                    ""type"": ""Value"",
                    ""id"": ""8388e3e6-4706-40d3-8add-6d5aec664d70"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ThrowAndPick_Left"",
                    ""type"": ""Button"",
                    ""id"": ""6c05597d-a5ed-4226-a6cb-b4012db42db5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ThrowAndPick_Right"",
                    ""type"": ""Button"",
                    ""id"": ""7e54c571-98db-4bed-a42d-4fc2eb9c53f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""98de6537-a427-46d1-a0b3-1295a87443c9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Melee"",
                    ""type"": ""Button"",
                    ""id"": ""7e3bba5b-9bb1-4f80-ac47-23dd87a3fb72"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""2f6400e4-d42d-4dfa-a5d2-8766f74a5be3"",
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
                    ""id"": ""f411067f-da2a-4cf9-a338-f7c95e92bde7"",
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
                    ""id"": ""6d28e427-980b-422c-bae8-b057536710a2"",
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
                    ""id"": ""a909859d-3e6b-407c-b987-ba446c7d64b5"",
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
                    ""id"": ""8cb62bc4-90c8-45e8-859c-d41cce66db26"",
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
                    ""id"": ""760fea5f-fdd5-4391-90b2-8540b1f9b4a5"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7af38b93-49b1-46ac-a005-b92af700a851"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c13a4a13-08d1-4725-a494-816a42ac8b1d"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1fa56bc8-18cf-47f3-945a-0c77486d5bcb"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowAndPick_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f92038fc-402a-4176-924e-ee968bf3cd5a"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ThrowAndPick_Right"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c21eb57c-f12b-446e-ae09-260842bc713f"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2513ddee-90a3-47fc-9c9c-0aff66b66f49"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Melee"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""9ba888dc-0839-4708-ab6e-9523d96e0393"",
            ""actions"": [
                {
                    ""name"": ""ViewTypeSwitch"",
                    ""type"": ""Value"",
                    ""id"": ""0ee6869a-dae2-4d11-a492-7b44907ab601"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6be790fd-d055-4799-8a18-589cefbab273"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ViewTypeSwitch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Move = m_Player.FindAction("Move", throwIfNotFound: true);
        m_Player_Dash = m_Player.FindAction("Dash", throwIfNotFound: true);
        m_Player_Shoot_Left = m_Player.FindAction("Shoot_Left", throwIfNotFound: true);
        m_Player_Shoot_Right = m_Player.FindAction("Shoot_Right", throwIfNotFound: true);
        m_Player_ThrowAndPick_Left = m_Player.FindAction("ThrowAndPick_Left", throwIfNotFound: true);
        m_Player_ThrowAndPick_Right = m_Player.FindAction("ThrowAndPick_Right", throwIfNotFound: true);
        m_Player_Interact = m_Player.FindAction("Interact", throwIfNotFound: true);
        m_Player_Melee = m_Player.FindAction("Melee", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_ViewTypeSwitch = m_Camera.FindAction("ViewTypeSwitch", throwIfNotFound: true);
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

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Move;
    private readonly InputAction m_Player_Dash;
    private readonly InputAction m_Player_Shoot_Left;
    private readonly InputAction m_Player_Shoot_Right;
    private readonly InputAction m_Player_ThrowAndPick_Left;
    private readonly InputAction m_Player_ThrowAndPick_Right;
    private readonly InputAction m_Player_Interact;
    private readonly InputAction m_Player_Melee;
    public struct PlayerActions
    {
        private @PlayerInputConfig m_Wrapper;
        public PlayerActions(@PlayerInputConfig wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Player_Move;
        public InputAction @Dash => m_Wrapper.m_Player_Dash;
        public InputAction @Shoot_Left => m_Wrapper.m_Player_Shoot_Left;
        public InputAction @Shoot_Right => m_Wrapper.m_Player_Shoot_Right;
        public InputAction @ThrowAndPick_Left => m_Wrapper.m_Player_ThrowAndPick_Left;
        public InputAction @ThrowAndPick_Right => m_Wrapper.m_Player_ThrowAndPick_Right;
        public InputAction @Interact => m_Wrapper.m_Player_Interact;
        public InputAction @Melee => m_Wrapper.m_Player_Melee;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Dash.started += instance.OnDash;
            @Dash.performed += instance.OnDash;
            @Dash.canceled += instance.OnDash;
            @Shoot_Left.started += instance.OnShoot_Left;
            @Shoot_Left.performed += instance.OnShoot_Left;
            @Shoot_Left.canceled += instance.OnShoot_Left;
            @Shoot_Right.started += instance.OnShoot_Right;
            @Shoot_Right.performed += instance.OnShoot_Right;
            @Shoot_Right.canceled += instance.OnShoot_Right;
            @ThrowAndPick_Left.started += instance.OnThrowAndPick_Left;
            @ThrowAndPick_Left.performed += instance.OnThrowAndPick_Left;
            @ThrowAndPick_Left.canceled += instance.OnThrowAndPick_Left;
            @ThrowAndPick_Right.started += instance.OnThrowAndPick_Right;
            @ThrowAndPick_Right.performed += instance.OnThrowAndPick_Right;
            @ThrowAndPick_Right.canceled += instance.OnThrowAndPick_Right;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @Melee.started += instance.OnMelee;
            @Melee.performed += instance.OnMelee;
            @Melee.canceled += instance.OnMelee;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Dash.started -= instance.OnDash;
            @Dash.performed -= instance.OnDash;
            @Dash.canceled -= instance.OnDash;
            @Shoot_Left.started -= instance.OnShoot_Left;
            @Shoot_Left.performed -= instance.OnShoot_Left;
            @Shoot_Left.canceled -= instance.OnShoot_Left;
            @Shoot_Right.started -= instance.OnShoot_Right;
            @Shoot_Right.performed -= instance.OnShoot_Right;
            @Shoot_Right.canceled -= instance.OnShoot_Right;
            @ThrowAndPick_Left.started -= instance.OnThrowAndPick_Left;
            @ThrowAndPick_Left.performed -= instance.OnThrowAndPick_Left;
            @ThrowAndPick_Left.canceled -= instance.OnThrowAndPick_Left;
            @ThrowAndPick_Right.started -= instance.OnThrowAndPick_Right;
            @ThrowAndPick_Right.performed -= instance.OnThrowAndPick_Right;
            @ThrowAndPick_Right.canceled -= instance.OnThrowAndPick_Right;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @Melee.started -= instance.OnMelee;
            @Melee.performed -= instance.OnMelee;
            @Melee.canceled -= instance.OnMelee;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private List<ICameraActions> m_CameraActionsCallbackInterfaces = new List<ICameraActions>();
    private readonly InputAction m_Camera_ViewTypeSwitch;
    public struct CameraActions
    {
        private @PlayerInputConfig m_Wrapper;
        public CameraActions(@PlayerInputConfig wrapper) { m_Wrapper = wrapper; }
        public InputAction @ViewTypeSwitch => m_Wrapper.m_Camera_ViewTypeSwitch;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void AddCallbacks(ICameraActions instance)
        {
            if (instance == null || m_Wrapper.m_CameraActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CameraActionsCallbackInterfaces.Add(instance);
            @ViewTypeSwitch.started += instance.OnViewTypeSwitch;
            @ViewTypeSwitch.performed += instance.OnViewTypeSwitch;
            @ViewTypeSwitch.canceled += instance.OnViewTypeSwitch;
        }

        private void UnregisterCallbacks(ICameraActions instance)
        {
            @ViewTypeSwitch.started -= instance.OnViewTypeSwitch;
            @ViewTypeSwitch.performed -= instance.OnViewTypeSwitch;
            @ViewTypeSwitch.canceled -= instance.OnViewTypeSwitch;
        }

        public void RemoveCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICameraActions instance)
        {
            foreach (var item in m_Wrapper.m_CameraActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CameraActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CameraActions @Camera => new CameraActions(this);
    public interface IPlayerActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnDash(InputAction.CallbackContext context);
        void OnShoot_Left(InputAction.CallbackContext context);
        void OnShoot_Right(InputAction.CallbackContext context);
        void OnThrowAndPick_Left(InputAction.CallbackContext context);
        void OnThrowAndPick_Right(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnMelee(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnViewTypeSwitch(InputAction.CallbackContext context);
    }
}
