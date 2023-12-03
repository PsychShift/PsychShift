using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using System.Data.Common;

public class InputManager : MonoBehaviour
{
    //[SerializeField] private CinemachinePOVExtension cinemachinePOVExtension;
    private static InputManager _instance;
    
    public static InputManager Instance {
        get {
            return _instance;
        }
    }
    public PlayerInput PlayerInput { get; private set; }

    public InputAction MoveAction { get; set; }
    public InputAction LookAction { get; set; }
    public InputAction JumpAction { get; set; }
    public InputAction ShootAction { get; set; }
    public InputAction SwitchAction { get; set; }
    #region Normal Controls
    public InputActionMap StandardActionMap { get; private set; }
    public InputAction StandardMoveAction { get; private set; }
    public InputAction StandardLookAction { get; private set; }
    public InputAction StandardJumpAction { get; private set; }
    public InputAction SlowAction { get; private set; }
    public InputAction StandardShootAction { get; private set; }
    public InputAction StandardSwitchAction {get; private set;}//Kevin Added this //Action to switch static and flow mode
    public InputAction StandardPauseAction {get; private set;}
    #endregion

    #region Slow Controls
    public InputActionMap SlowActionMap { get; private set; }
    public InputAction SlowMoveAction { get; private set; }
    public InputAction SlowLookAction { get; private set; }
    public InputAction SlowJumpAction { get; private set; }
    public InputAction SwapSlowAction { get; private set; }
    public InputAction UnSlowAction { get; private set; }
    public InputAction SlowShootAction { get; private set; }
    public InputAction ManipulateAction { get; private set; }
    public InputAction SlowSwitchAction {get; private set;}//Kevin Added this //Action to switch static and flow mode
    public InputAction SlowPauseAction {get; private set;}
    #endregion

    #region UI Controls
    public InputActionMap UIActionMap { get; private set; }
    public InputAction UIPauseAction { get; private set; }
    #endregion

    public bool IsJumpPressed { get; private set; }
    public bool IsShootingHeld { get; private set; }
    public delegate void SwapPressedHandler();
    public delegate void ManipulatePressedHandler();
    public delegate void ShootPressedHandler();
    public event SwapPressedHandler OnSwapPressed;
    public event ManipulatePressedHandler OnManipulatePressed;
    public event ShootPressedHandler OnShootPressed;
    public event Action<bool> OnSlowActionStateChanged;
    public event Action<bool> OnSwitchPressed;
    public event Action OnPausePressed;
    
    //ART MUSEUM
    public InputAction ShaderIsPressed{get; private set;}
    public InputAction ShaderIsPressedSlow{get; private set;}

    private void Awake() {
        PlayerInput = GetComponent<PlayerInput>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    bool switchFromKeyboard = false;
    bool switchFromController = false;
    private void Update()
    {
        /* if(PlayerInput.currentControlScheme == "Keyboard" && switchFromController)
        {
            // Set to PC sensitivity
            cinemachinePOVExtension.horizontalSpeed = 10f;
            cinemachinePOVExtension.verticalSpeed = 10f;
            switchFromController = false;
            switchFromKeyboard = true;
        }
        else if(PlayerInput.currentControlScheme == "Controller" && switchFromKeyboard)
        {
            // Set to Controller sensitivity
            cinemachinePOVExtension.horizontalSpeed = 180f;
            cinemachinePOVExtension.verticalSpeed = 140f;
            switchFromController = true;
            switchFromKeyboard = false;
        } */
    }

    private void OnEnable() {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        StandardActionMap = PlayerInput.actions.FindActionMap("Controls");
        SlowActionMap = PlayerInput.actions.FindActionMap("Slow");
        UIActionMap = PlayerInput.actions.FindActionMap("UI");

        #region Standard Controls
        StandardMoveAction = PlayerInput.actions[StandardActionMap.name + "/Move"];
        StandardLookAction = PlayerInput.actions[StandardActionMap.name + "/Look"];
        StandardJumpAction = PlayerInput.actions[StandardActionMap.name + "/Jump"];
        StandardShootAction = PlayerInput.actions[StandardActionMap.name + "/Shoot"];//Kevin added this shooting thing
        SlowAction = PlayerInput.actions[StandardActionMap.name + "/Slow"];
        StandardPauseAction = PlayerInput.actions[StandardActionMap.name + "/Pause"];
        //swap input Kevin added this
        StandardSwitchAction = PlayerInput.actions[StandardActionMap.name + "/Switch"];
        ShaderIsPressed = PlayerInput.actions[StandardActionMap.name + "/Shader"];
        ShaderIsPressedSlow = PlayerInput.actions[StandardActionMap.name + "/Shader"];
        #endregion
        
        
        #region Slow Controls
        SlowMoveAction = PlayerInput.actions[SlowActionMap.name + "/Move"];
        SlowLookAction = PlayerInput.actions[SlowActionMap.name + "/Look"];
        SlowJumpAction = PlayerInput.actions[SlowActionMap.name + "/Jump"];
        SlowShootAction = PlayerInput.actions[SlowActionMap.name + "/Shoot"];
        SwapSlowAction = PlayerInput.actions[SlowActionMap.name + "/MindSwap"];
        ManipulateAction = PlayerInput.actions[SlowActionMap.name + "/Manipulate"];
        UnSlowAction = PlayerInput.actions[SlowActionMap.name + "/Unslow"];
        SlowSwitchAction = PlayerInput.actions[SlowActionMap.name + "/Switch"];
        SlowPauseAction = PlayerInput.actions[SlowActionMap.name + "/Pause"];
        #endregion

        #region UI Controls
        UIPauseAction = PlayerInput.actions[UIActionMap.name + "/Pause"];
        #endregion
    
        /* PlayerInput.SwitchCurrentControlScheme("Controller");
        Debug.Log(PlayerInput.currentControlScheme); */

        SwapControlMap(ActionMapEnum.standard);

        StandardJumpAction.performed += OnJump;
        StandardJumpAction.canceled += OnJump;
        SlowJumpAction.performed += OnJump;
        SlowJumpAction.canceled += OnJump;
        SlowAction.started += PressedSlow;
        UnSlowAction.started += ReleasedSlow;
        SwapSlowAction.started += PressedSwap;
        ManipulateAction.started += PressedManipulate;
        SwitchAction.started += OnSwitch;
        StandardShootAction.started += PressedShoot;
        SlowShootAction.started += PressedShoot;
        StandardShootAction.canceled += PressedShoot;
        SlowShootAction.canceled += PressedShoot;
        StandardPauseAction.started += PressedPause;
        SlowPauseAction.started += PressedPause;
        UIPauseAction.started += PressedPause;


        // Camera Sensitivity Stuff
        switchFromController = true;
    }
    private void OnDisable() {
        StandardJumpAction.performed -= OnJump;
        StandardJumpAction.canceled -= OnJump;
        SlowJumpAction.performed -= OnJump;
        SlowJumpAction.canceled -= OnJump;
        SlowAction.started -= PressedSlow;
        UnSlowAction.started -= ReleasedSlow;
        SwapSlowAction.started -= PressedSwap;
        ManipulateAction.started -= PressedManipulate;
        SwitchAction.started -= OnSwitch;
        StandardShootAction.started -= PressedShoot;
        SlowShootAction.started -= PressedShoot;
        StandardPauseAction.started-=PressedPause;
        SlowPauseAction.started-= PressedPause;
        UIPauseAction.started -= PressedPause;
    }

    public Vector2 GetPlayerMovement() {
        return MoveAction.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta() {
        return LookAction.ReadValue<Vector2>();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        IsJumpPressed = context.ReadValueAsButton();
    }

    private void PressedSwap(InputAction.CallbackContext context) {
        OnSwapPressed?.Invoke();
    }
    
    private void PressedSlow(InputAction.CallbackContext context) {
        OnSlowActionStateChanged?.Invoke(true);
    }
    private void ReleasedSlow(InputAction.CallbackContext context) {
        OnSlowActionStateChanged?.Invoke(false);
    }

    private void PressedManipulate(InputAction.CallbackContext context)
    {
        OnManipulatePressed?.Invoke();
    }
    private void PressedPause(InputAction.CallbackContext context)
    {
        OnPausePressed?.Invoke();
    }
    private bool _switch = false;
    private void OnSwitch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if(_switch)
            {
                OnSwitchPressed?.Invoke(true);
            }
            else
            OnSwitchPressed?.Invoke(false);
            _switch = !_switch;
        }
        //else if (context.canceled)
        //{
            //OnSwitchPressed?.Invoke(false);
        //}

    }

    public void SwapControlMap(ActionMapEnum currentMap)
{
    switch (currentMap)
    {
        case ActionMapEnum.standard:
            PlayerInput.SwitchCurrentActionMap(StandardActionMap.name);
            ShootAction = StandardShootAction;//added by kevin
            MoveAction = StandardMoveAction;
            LookAction = StandardLookAction;
            SwitchAction = StandardSwitchAction;
            break;
        case ActionMapEnum.slow:
            PlayerInput.SwitchCurrentActionMap(SlowActionMap.name);
            ShootAction = SlowShootAction;//added by kevin
            MoveAction = SlowMoveAction;
            LookAction = SlowLookAction;
            SwitchAction = SlowSwitchAction;
            break;
        case ActionMapEnum.ui:
            PlayerInput.SwitchCurrentActionMap(UIActionMap.name);
            break;
        default:
            Debug.LogWarning("Unknown action map: " + currentMap);
            break;
    }
}


    //KEVIN ADDED THIS I SORRY IF BROKE
    public void PressedShoot(InputAction.CallbackContext context)
    {
        OnShootPressed?.Invoke();
        IsShootingHeld = context.ReadValueAsButton();
    }

    public bool PlayerSwitchedModeThisFrame()//Press L shift or L bump to swap static/flow//Kevin Added this
    {
        return SwitchAction.triggered;
    }
    public bool PlayerSwitcherShader()
    {
        if(ShaderIsPressed.triggered)
            return true;
        else if(ShaderIsPressedSlow.triggered)
            return true;
        else
            return false;
    }

}

public enum ActionMapEnum
{
    standard,
    slow,
    ui
}