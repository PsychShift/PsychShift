using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

public class InputManager : MonoBehaviour
{
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
    #endregion

    public bool IsJumpPressed { get; private set; }
    public event Action OnSwapPressed;
    public event Action OnManipulatePressed;
    public event Action OnShootPressed;
    public event Action<bool> OnSlowActionStateChanged;
    public event Action<bool> OnSwitchPressed;

    private void Awake() {
        PlayerInput = GetComponent<PlayerInput>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

        #region Standard Controls
        StandardMoveAction = PlayerInput.actions[StandardActionMap.name + "/Move"];
        StandardLookAction = PlayerInput.actions[StandardActionMap.name + "/Look"];
        StandardJumpAction = PlayerInput.actions[StandardActionMap.name + "/Jump"];
        StandardShootAction = PlayerInput.actions[StandardActionMap.name + "/Shoot"];//Kevin added this shooting thing
        SlowAction = PlayerInput.actions[StandardActionMap.name + "/Slow"];
        //swap input Kevin added this
        StandardSwitchAction = PlayerInput.actions[StandardActionMap.name + "/Switch"];
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
        #endregion



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
    private bool _switch = false;
    private void OnSwitch(InputAction.CallbackContext context)
    {
        _switch = !_switch;
        OnSwitchPressed?.Invoke(_switch);
    }

    public void SwapControlMap(ActionMapEnum currentMap)
{
    switch (currentMap)
    {
        case ActionMapEnum.standard:
            PlayerInput.SwitchCurrentActionMap(StandardActionMap.name);
            MoveAction = StandardMoveAction;
            LookAction = StandardLookAction;
            SwitchAction = StandardSwitchAction;
            break;
        case ActionMapEnum.slow:
            PlayerInput.SwitchCurrentActionMap(SlowActionMap.name);
            MoveAction = SlowMoveAction;
            LookAction = SlowLookAction;
            SwitchAction = SlowSwitchAction;
            break;
        case ActionMapEnum.ui:
            // Handle switching to the UI action map if needed.
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
    }

    public bool PlayerSwitchedModeThisFrame()//Press L shift or L bump to swap static/flow//Kevin Added this
    {
        return SwitchAction.triggered;
    }

}

public enum ActionMapEnum
{
    standard,
    slow,
    ui
}