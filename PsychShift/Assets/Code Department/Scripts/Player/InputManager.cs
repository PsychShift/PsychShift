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

    #region Normal Controls
    public InputActionMap StandardActionMap { get; private set; }
    public InputAction MoveAction { get; private set; }
    public InputAction RunAction { get; private set; }
    public InputAction LookAction { get; private set; }
    public InputAction JumpAction { get; private set; }
    public InputAction SlowAction { get; private set; }
    public InputAction ShootAction { get; private set; }
    #endregion

    #region Slow Controls
    public InputActionMap SlowActionMap { get; private set; }
    public InputAction SwapSlowAction { get; private set; }
    public InputAction UnSlowAction { get; private set; }
    public InputAction ManipulateAction { get; private set; }
    #endregion

    public bool IsJumpPressed { get; private set; }
    public event Action OnSwapPressed;
    public event Action OnManipulatePressed;
    public event Action<bool> OnSlowActionStateChanged;

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
        MoveAction = PlayerInput.actions[StandardActionMap.name + "/Move"];
        LookAction = PlayerInput.actions[StandardActionMap.name + "/Look"];
        RunAction = PlayerInput.actions[StandardActionMap.name + "/Run"];
        JumpAction = PlayerInput.actions[StandardActionMap.name + "/Jump"];
        ShootAction = PlayerInput.actions[StandardActionMap.name + "/Shoot"];//Kevin added this shooting thing
        SlowAction = PlayerInput.actions[StandardActionMap.name + "/Slow"];
        #endregion
        
        JumpAction.performed += OnJump;
        JumpAction.canceled += OnJump;
        
        #region Slow Controls
        SwapSlowAction = PlayerInput.actions[SlowActionMap.name + "/MindSwap"];
        ManipulateAction = PlayerInput.actions[SlowActionMap.name + "/Manipulate"];
        UnSlowAction = PlayerInput.actions[SlowActionMap.name + "/Unslow"];
        #endregion

        SlowAction.started += PressedSlow;
        UnSlowAction.started += ReleasedSlow;
        SwapSlowAction.started += PressedSwap;
        ManipulateAction.started += PressedManipulate;

    }
    private void OnDisable() {
        JumpAction.performed -= OnJump;
        JumpAction.canceled -= OnJump; 
        SlowAction.started -= PressedSlow;
        UnSlowAction.started -= ReleasedSlow;
        SwapSlowAction.started -= PressedSwap;
        ManipulateAction.started -= PressedManipulate;
    }

    public Vector2 GetPlayerMovement() {
        return MoveAction.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta() {
        return LookAction.ReadValue<Vector2>();
    }
    private void OnJump(InputAction.CallbackContext context)
    {
        //IsJumpPressed = context.ReadValueAsButton();
        IsJumpPressed = !IsJumpPressed;
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


    //KEVIN ADDED THIS I SORRY IF BROKE
    public bool PlayerShotThisFrame() {
        return ShootAction.triggered;
    }

}