using System;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(InputManager))]
public class PlayerStateMachine : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject tempCharacter;
    private StateMachine.StateMachine stateMachine;
    private InputManager inputManager;

    public CharacterInfo currentCharacter { get; set; }
    public Transform cameraTransform { get; private set; }

    [Header("Movement Variables")]
    public float walkSpeed = 20;
    public float smoothInputSpeed;

    public Vector2 currentInputVector { get; set; }
    [HideInInspector] public Vector2 smoothInputVelocity;
    public Vector3 move { get; set; }
    

    [Header("Jump Variables")]
    public float jumpHeight = 4f;
    public float gravityValue = -9.81f;
    [HideInInspector] public Vector3 playerVelocity;
    

    private void Awake()
    {
        cameraTransform = Camera.main.transform;
        SwapCharacter(tempCharacter);
        inputManager = GetComponent<InputManager>();
        stateMachine = new StateMachine.StateMachine();

        // Create instances of root states
        var groundState = new GroundedState(this, stateMachine);
        var fallState = new FallState(this, stateMachine);
        var jumpState = new JumpState(this, stateMachine);

        // Create instances of sub-states
        var idleState = new IdleState(this);
        var walkState = new WalkState(this);
        var runState = new RunState(this);
        var crouchState = new CrouchState(this);    

        // Makes it easier to add transitions (less text per line)
        void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition); // If a condition meets switch from 'from' state to 'to' state (Root state only)
        //void AAt(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition); // If 

        #region Root State Transitions
        // Leave Ground State
        AT(groundState, jumpState, Jumped());
        AT(groundState, fallState, Falling());
        // Leave Jump State
        AT(jumpState, groundState, Grounded());
        AT(jumpState, fallState, Falling());
        // Leave Fall State
        AT(fallState, groundState, Grounded());
        #endregion

        #region Idle State Transitions
        AT(idleState, walkState, Walked());
        AT(idleState, runState, Running());
        #endregion
        #region Walk State Transitions
        AT(walkState, runState, Running());
        AT(walkState, idleState, Stopped());
        #endregion
        #region Run State Transitions
        AT(runState, idleState, Stopped());
        AT(runState, walkState, Walked());
        #endregion
        
        #region Assign Substates to Rootstates
        groundState.AddSubState(idleState);
        groundState.AddSubState(walkState);
        groundState.AddSubState(runState);
        //groundState.AddSubState(crouchState);
        groundState.PrepareSubStates();
        groundState.SetDefaultSubState(idleState);

        jumpState.AddSubState(idleState);
        jumpState.AddSubState(walkState);
        jumpState.AddSubState(runState);
        jumpState.PrepareSubStates();
        jumpState.SetDefaultSubState(idleState);

        fallState.AddSubState(idleState);
        fallState.AddSubState(walkState);
        fallState.AddSubState(runState);
        fallState.PrepareSubStates();
        fallState.SetDefaultSubState(idleState);
        #endregion

        // Root State Conditions
        Func<bool> Jumped() => () => inputManager.jumpAction.triggered && currentCharacter.controller.isGrounded;
        Func<bool> Falling() => () => playerVelocity.y < 0 && !currentCharacter.controller.isGrounded;
        Func<bool> Grounded() => () => currentCharacter.controller.isGrounded;

        // Sub State Conditions
        Func<bool> Walked() => () => inputManager.moveAction.triggered && !inputManager.runAction.triggered;
        Func<bool> Stopped() => () => inputManager.moveAction.ReadValue<Vector2>().magnitude == 0;
        Func<bool> Running() => () => inputManager.moveAction.triggered && inputManager.runAction.triggered;

        stateMachine.SetState(groundState);
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
