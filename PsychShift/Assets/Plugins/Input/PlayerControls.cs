//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Plugins/Input/PlayerControls.inputactions
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

public partial class @PlayerControls: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Controls"",
            ""id"": ""e1b58372-dfc1-4a27-8151-eecc221663c4"",
            ""actions"": [
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""a065e55b-c81e-4ea9-9851-6992d70a45f4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""7c6b27aa-4a17-4a27-9ba5-94f099f5032d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""3d9fa3fc-5b84-4aa5-997d-aca0ce119abf"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MindSwap"",
                    ""type"": ""Button"",
                    ""id"": ""138cc8ed-df22-4c9b-8fcf-5f8a49be218f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Slow"",
                    ""type"": ""Button"",
                    ""id"": ""f8531946-fa6c-4235-bdef-093284f13437"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Shoot"",
                    ""type"": ""Button"",
                    ""id"": ""289682a5-a208-4c82-825e-d7d49303efde"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""919ea29d-02e5-4ec4-995b-1db7cab2119f"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0e4f2a0d-10b4-4b53-9891-a6827d860049"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2c961499-6718-4495-98a6-ad0db14ac3dc"",
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
                    ""id"": ""8704b7b9-556d-47aa-bbf8-3874cbd0b74c"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""WASD"",
                    ""id"": ""1c2f7619-5083-45ad-bb79-a45aa8776859"",
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
                    ""id"": ""ee75e91a-ab6d-4c4d-8ab8-eb429044bd51"",
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
                    ""id"": ""2d1ae3bb-65ef-447b-9f2d-d1054bc21484"",
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
                    ""id"": ""1cc402ee-710b-427f-9b54-e720ff3671da"",
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
                    ""id"": ""a7c483e8-7dce-46a4-9a53-91da359165a4"",
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
                    ""id"": ""5eec18ab-5392-4fd1-a568-7530372963d0"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a13c0e81-8b16-494c-8f60-8977bf78d788"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MindSwap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""89d993b1-2a0e-4103-9e95-fe5c108c985f"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MindSwap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d17a01b1-df5b-400c-a785-9c19f2536450"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a09f0a0f-472a-4bee-90c1-224bd9a8777d"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Slow"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""01a2118b-797a-4553-93ad-378353bf8a43"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Shoot"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Slow"",
            ""id"": ""17ad749a-b79b-4c0c-91b4-214b9c9fdbd4"",
            ""actions"": [
                {
                    ""name"": ""Mind Swap"",
                    ""type"": ""Button"",
                    ""id"": ""daaa2f70-b226-466b-b0b5-e9cd2d328651"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pull"",
                    ""type"": ""Button"",
                    ""id"": ""010324a3-915f-4dc1-8c65-1087c521199f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Push"",
                    ""type"": ""Button"",
                    ""id"": ""4e557698-e793-4a03-aaff-6622df1944f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""e10fb38b-3eec-4f71-80e2-c2aac79aef9a"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Mind Swap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4948d8c4-cf6c-4c21-a4b5-5433b1104ed7"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pull"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""02501a03-9374-40b5-856b-cdb52bcf6a01"",
                    ""path"": """",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Push"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Controls
        m_Controls = asset.FindActionMap("Controls", throwIfNotFound: true);
        m_Controls_Look = m_Controls.FindAction("Look", throwIfNotFound: true);
        m_Controls_Move = m_Controls.FindAction("Move", throwIfNotFound: true);
        m_Controls_Jump = m_Controls.FindAction("Jump", throwIfNotFound: true);
        m_Controls_MindSwap = m_Controls.FindAction("MindSwap", throwIfNotFound: true);
        m_Controls_Slow = m_Controls.FindAction("Slow", throwIfNotFound: true);
        m_Controls_Shoot = m_Controls.FindAction("Shoot", throwIfNotFound: true);
        // Slow
        m_Slow = asset.FindActionMap("Slow", throwIfNotFound: true);
        m_Slow_MindSwap = m_Slow.FindAction("Mind Swap", throwIfNotFound: true);
        m_Slow_Pull = m_Slow.FindAction("Pull", throwIfNotFound: true);
        m_Slow_Push = m_Slow.FindAction("Push", throwIfNotFound: true);
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

    // Controls
    private readonly InputActionMap m_Controls;
    private List<IControlsActions> m_ControlsActionsCallbackInterfaces = new List<IControlsActions>();
    private readonly InputAction m_Controls_Look;
    private readonly InputAction m_Controls_Move;
    private readonly InputAction m_Controls_Jump;
    private readonly InputAction m_Controls_MindSwap;
    private readonly InputAction m_Controls_Slow;
    private readonly InputAction m_Controls_Shoot;
    public struct ControlsActions
    {
        private @PlayerControls m_Wrapper;
        public ControlsActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Look => m_Wrapper.m_Controls_Look;
        public InputAction @Move => m_Wrapper.m_Controls_Move;
        public InputAction @Jump => m_Wrapper.m_Controls_Jump;
        public InputAction @MindSwap => m_Wrapper.m_Controls_MindSwap;
        public InputAction @Slow => m_Wrapper.m_Controls_Slow;
        public InputAction @Shoot => m_Wrapper.m_Controls_Shoot;
        public InputActionMap Get() { return m_Wrapper.m_Controls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControlsActions set) { return set.Get(); }
        public void AddCallbacks(IControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_ControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ControlsActionsCallbackInterfaces.Add(instance);
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @MindSwap.started += instance.OnMindSwap;
            @MindSwap.performed += instance.OnMindSwap;
            @MindSwap.canceled += instance.OnMindSwap;
            @Slow.started += instance.OnSlow;
            @Slow.performed += instance.OnSlow;
            @Slow.canceled += instance.OnSlow;
            @Shoot.started += instance.OnShoot;
            @Shoot.performed += instance.OnShoot;
            @Shoot.canceled += instance.OnShoot;
        }

        private void UnregisterCallbacks(IControlsActions instance)
        {
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @MindSwap.started -= instance.OnMindSwap;
            @MindSwap.performed -= instance.OnMindSwap;
            @MindSwap.canceled -= instance.OnMindSwap;
            @Slow.started -= instance.OnSlow;
            @Slow.performed -= instance.OnSlow;
            @Slow.canceled -= instance.OnSlow;
            @Shoot.started -= instance.OnShoot;
            @Shoot.performed -= instance.OnShoot;
            @Shoot.canceled -= instance.OnShoot;
        }

        public void RemoveCallbacks(IControlsActions instance)
        {
            if (m_Wrapper.m_ControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_ControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ControlsActions @Controls => new ControlsActions(this);

    // Slow
    private readonly InputActionMap m_Slow;
    private List<ISlowActions> m_SlowActionsCallbackInterfaces = new List<ISlowActions>();
    private readonly InputAction m_Slow_MindSwap;
    private readonly InputAction m_Slow_Pull;
    private readonly InputAction m_Slow_Push;
    public struct SlowActions
    {
        private @PlayerControls m_Wrapper;
        public SlowActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @MindSwap => m_Wrapper.m_Slow_MindSwap;
        public InputAction @Pull => m_Wrapper.m_Slow_Pull;
        public InputAction @Push => m_Wrapper.m_Slow_Push;
        public InputActionMap Get() { return m_Wrapper.m_Slow; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(SlowActions set) { return set.Get(); }
        public void AddCallbacks(ISlowActions instance)
        {
            if (instance == null || m_Wrapper.m_SlowActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_SlowActionsCallbackInterfaces.Add(instance);
            @MindSwap.started += instance.OnMindSwap;
            @MindSwap.performed += instance.OnMindSwap;
            @MindSwap.canceled += instance.OnMindSwap;
            @Pull.started += instance.OnPull;
            @Pull.performed += instance.OnPull;
            @Pull.canceled += instance.OnPull;
            @Push.started += instance.OnPush;
            @Push.performed += instance.OnPush;
            @Push.canceled += instance.OnPush;
        }

        private void UnregisterCallbacks(ISlowActions instance)
        {
            @MindSwap.started -= instance.OnMindSwap;
            @MindSwap.performed -= instance.OnMindSwap;
            @MindSwap.canceled -= instance.OnMindSwap;
            @Pull.started -= instance.OnPull;
            @Pull.performed -= instance.OnPull;
            @Pull.canceled -= instance.OnPull;
            @Push.started -= instance.OnPush;
            @Push.performed -= instance.OnPush;
            @Push.canceled -= instance.OnPush;
        }

        public void RemoveCallbacks(ISlowActions instance)
        {
            if (m_Wrapper.m_SlowActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ISlowActions instance)
        {
            foreach (var item in m_Wrapper.m_SlowActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_SlowActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public SlowActions @Slow => new SlowActions(this);
    public interface IControlsActions
    {
        void OnLook(InputAction.CallbackContext context);
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnMindSwap(InputAction.CallbackContext context);
        void OnSlow(InputAction.CallbackContext context);
        void OnShoot(InputAction.CallbackContext context);
    }
    public interface ISlowActions
    {
        void OnMindSwap(InputAction.CallbackContext context);
        void OnPull(InputAction.CallbackContext context);
        void OnPush(InputAction.CallbackContext context);
    }
}
