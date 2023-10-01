using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public static InputManager Instance {
        get {
            return _instance;
        }
    }
    public PlayerInput playerInput { get; private set; }

    #region Normal Controls
    public InputActionMap standardActionMap;
    public InputAction moveAction { get; private set; }
    public InputAction runAction { get; private set; }
    public InputAction lookAction { get; private set; }
    public InputAction jumpAction { get; private set; }
    public InputAction slowAction { get; private set; }
    public InputAction shootAction { get; private set; }
    #endregion

    #region Slow Controls
    public InputActionMap slowActionMap { get; private set; }
    public InputAction swapSlowAction { get; private set; }
    public InputAction unSlowAction { get; private set; }
    #endregion

    public event Action OnSwapPressed;
    public event Action<bool> OnSlowActionStateChanged;

    private void Awake() {
        playerInput = GetComponent<PlayerInput>();
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
        standardActionMap = playerInput.actions.FindActionMap("Controls");
        slowActionMap = playerInput.actions.FindActionMap("Slow");
        Debug.Log($"{standardActionMap.name} & {slowActionMap.name}");

        #region Standard Controls
        moveAction = playerInput.actions[standardActionMap.name + "/Move"];
        lookAction = playerInput.actions[standardActionMap.name + "/Look"];
        runAction = playerInput.actions[standardActionMap.name + "/Run"];
        jumpAction = playerInput.actions[standardActionMap.name + "/Jump"];
        slowAction = playerInput.actions[standardActionMap.name + "/Slow"];
        shootAction = playerInput.actions[standardActionMap.name + "/Shoot"];//Kevin added this shooting thing
        #endregion
        
        #region Slow Controls
        swapSlowAction = playerInput.actions[slowActionMap.name + "/MindSwap"];
        unSlowAction = playerInput.actions[slowActionMap.name + "/Unslow"];
        #endregion

        slowAction.started += PressedSlow;
        unSlowAction.started += ReleasedSlow;
        swapSlowAction.started += PressedSwap;
    }
    private void OnDisable() {
        
        
    }

    public Vector2 GetPlayerMovement() {
        return moveAction.ReadValue<Vector2>();
    }
    public Vector2 GetMouseDelta() {
        return lookAction.ReadValue<Vector2>();
    }
    public bool PlayerJumpedThisFrame() {
        return jumpAction.triggered;
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


    //KEVIN ADDED THIS I SORRY IF BROKE
    public bool PlayerShotThisFrame() {
        return shootAction.triggered;
    }

}