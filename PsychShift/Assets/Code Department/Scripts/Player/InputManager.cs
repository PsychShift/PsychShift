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
    private PlayerInput playerInput;

    #region Normal Controls
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction slowAction;
    private InputAction swapAction;
    #endregion

    #region Slow Controls
    //private InputAction swapAction;
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
        
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];
        jumpAction = playerInput.actions["Jump"];
        slowAction = playerInput.actions["Slow"];
        swapAction = playerInput.actions["MindSwap"];

        slowAction.started += PressedSlow;
        slowAction.canceled += ReleasedSlow;
        swapAction.started += PressedSwap;
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
}