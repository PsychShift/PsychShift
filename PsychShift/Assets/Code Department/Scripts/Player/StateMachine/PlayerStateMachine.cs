using System;
/* using System.Collections;
using System.Collections.Generic; */
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(InputManager))]
public class PlayerStateMachine : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject tempCharacter;
    private StateMachine stateMachine;
    private InputManager inputManager;

    public CharacterInfo currentCharacter { get; set; }
    public Transform cameraTransform { get; private set; }

    [Header("Movement Variables")]
    public float walkSpeed = 20;
    public float smoothInputSpeed;

    public Vector2 currentInputVector { get; set; }
    [HideInInspector] public Vector2 smoothInputVelocity;
    

    [Header("Jump Variables")]
    public float jumpHeight = 4f;
    public float gravityValue = -9.81f;
    [HideInInspector] public Vector3 playerVelocity;
    

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        SwapCharacter(tempCharacter);
        inputManager = GetComponent<InputManager>();
        stateMachine = new StateMachine();

        // Create Instances of the states for the script to use
        // This uses a constructor made in the states script. The argument can be anything set in the script.
        var idleState = new IdleState(this);
        var walkState = new WalkState(this);
        var runState = new RunState(this);
        var crouchState = new CrouchState(this);
        var jumpState = new JumpState(this);
        var fallState = new FallState(this);
        var groundState = new GroundedState(this);
        

        // Makes it easier to add transitions (less text per line)
        void At(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition); // If a condition meets switch from 'from' state to 'to' state
        void AAt(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition); // If 


        #region Idle State Transitions
        At(idleState, walkState, Moved());
        At(idleState, jumpState, Jumped());
        #endregion
        #region Walk State Transitions
        At(walkState, jumpState, Jumped());
        At(walkState, idleState, Stopped());
        #endregion
        #region Jump State Transitions
        At(jumpState, idleState, Grounded());
        #endregion
        #region Fall State Transitions
        AAt(fallState, Falling());
        At(fallState, idleState, Grounded());
        #endregion

        // Root State Conditions
        Func<bool> Jumped() => () => inputManager.jumpAction.triggered && currentCharacter.controller.isGrounded;
        Func<bool> Falling() => () => playerVelocity.y < 0 && !currentCharacter.controller.isGrounded;
        Func<bool> Grounded() => () => currentCharacter.controller.isGrounded;

        // Sub State Conditions
        Func<bool> Moved() => () => inputManager.moveAction.triggered;
        Func<bool> Stopped() => () => inputManager.moveAction.ReadValue<Vector2>().magnitude == 0;
        //Func<bool> Running() => () => inputManager.runAction.triggered;

        stateMachine.SetState(idleState);
    }

    void Update()
    {
        stateMachine.Tick();
    }

    public void SwapCharacter(GameObject newCharacter)
    {
        if(newCharacter == null) return;
        currentCharacter = new CharacterInfo
        {
            characterContainer = newCharacter,
            model = newCharacter.transform.GetChild(1).gameObject,
            cameraRoot = newCharacter.transform.GetChild(0),
            controller = newCharacter.GetComponent<CharacterController>()
        };
        virtualCamera.Follow = currentCharacter.cameraRoot;
    }    
}
