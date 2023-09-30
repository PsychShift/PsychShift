using System;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(InputManager))]
public class PlayerStateMachine : MonoBehaviour
{
    #region References
    [Header("Look Speeds")]
    //[SerializeField] private float vertLookSpeed = 300, horzLookSpeed = 300, slowVertLookSpeed = 150, slowHorzLookSpeed = 150;
    public CinemachineVirtualCamera virtualCamera;
    private StateMachine.StateMachine stateMachine;
    private InputManager inputManager;
    [HideInInspector] public Transform cameraTransform;
    #endregion

    #region Movement Variables
    [Header("Movement Variables")]
    private Vector3 currentMovement;
    private Vector3 appliedMovement;
    public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; }}
    public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; }}
    public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; }}
    public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; }}

    public Vector3 InAirForward { get; set; }
    public Vector3 InAirRight { get; set; }
    [SerializeField] private float walkSpeed = 20;
    public float WalkSpeed { get { return walkSpeed; } }
    public float smoothInputSpeed;

    public Vector2 currentInputVector { get; set; }
    [HideInInspector] public Vector2 smoothInputVelocity;
    public Vector3 move { get; set; }
    
    [Header("Vaulting Variables")]
    [SerializeField] private LayerMask vaultLayers;
    public LayerMask VaultLayers { get { return vaultLayers; } }
    public bool IsVaulting { get; set; }

    [Header("Jump Variables")]
    public float jumpHeight = 4f;
    public float gravityValue = -9.81f;
    [SerializeField] float maxJumpHeight = 4.0f;
    [SerializeField] float maxJumpTime = 0.75f;
    private float initialJumpVelocity;
    private float initialJumpGravity;
    public float InitialJumpVelocity { get { return initialJumpVelocity; } }
    public float InitialJumpGravity { get { return initialJumpGravity; } }
    #endregion
    
    #region Swap Variables
    [SerializeField] public float swapDistance = 100f;
    public float SwapDistance { get { return swapDistance; } set { swapDistance = value; } }
    [SerializeField] private LayerMask swapableLayer;
    public LayerMask SwapableLayer { get { return swapableLayer; }}
    #endregion
    
    #region  Slow Variables
    private bool isSlowed = false;
    #endregion
    [SerializeField] private GameObject tempCharacter;
    public CharacterInfo currentCharacter { get; set; }
    private void Awake()
    {
        
        SetJumpVariables();
        cameraTransform = Camera.main.transform;
        cameraTransform.GetComponent<CinemachineBrain>().m_IgnoreTimeScale = false;
        currentCharacter = currentCharacter = new CharacterInfo
        {
            characterContainer = tempCharacter,
            model = tempCharacter.transform.GetChild(1).gameObject,
            cameraRoot = tempCharacter.transform.GetChild(0),
            controller = tempCharacter.GetComponent<CharacterController>()
        };
        virtualCamera.Follow = currentCharacter.cameraRoot;
        inputManager = GetComponent<InputManager>();
        stateMachine = new StateMachine.StateMachine();

        inputManager.OnSlowActionStateChanged += SlowMotion;
        inputManager.OnSwapPressed += SwapPressed;

        // Create instances of root states
        var slowState = new SlowState(this, stateMachine);
        var standardState = new StandardState(this, stateMachine);

        var groundState = new GroundedState(this, stateMachine);
        var fallState = new FallState(this, stateMachine);
        var jumpState = new JumpState(this, stateMachine);
        var vaultState = new VaultState(this);

        // Create instances of sub-states
        var idleState = new IdleState(this);
        var walkState = new WalkState(this);
        var runState = new RunState(this);
        var crouchState = new CrouchState(this);    

        // Makes it easier to add transitions (less text per line)
        void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition); // If a condition meets switch from 'from' state to 'to' state (Root state only)
        //void AAt(IState to, Func<bool> condition) => stateMachine.AddAnyTransition(to, condition); // If 

        #region Root State Transitions
        AT(standardState, slowState, Slowed());

        AT(slowState, standardState, NotSlowed());


        // Leave Ground State
        AT(groundState, jumpState, Jumped());
        AT(groundState, fallState, Falling());
        AT(groundState, vaultState, Vaulting());
        // Leave Jump State
        AT(jumpState, groundState, Grounded());
        AT(jumpState, fallState, Falling());
        AT(jumpState, vaultState, Vaulting());
        // Leave Fall State
        AT(fallState, groundState, Grounded());
        AT(fallState, vaultState, Vaulting());
        // Leave Vault State
        AT(vaultState, groundState, NotVaulting());
        AT(vaultState, fallState, NotVaulting());
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
        standardState.AddSubState(groundState);
        standardState.AddSubState(jumpState);
        standardState.AddSubState(fallState);
        standardState.PrepareSubStates();
        standardState.SetDefaultSubState(groundState);

        slowState.AddSubState(groundState);
        slowState.AddSubState(jumpState);
        slowState.AddSubState(fallState);
        slowState.PrepareSubStates();
        slowState.SetDefaultSubState(groundState);

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
        Func<bool> Falling() => () => AppliedMovementY < 0 && !currentCharacter.controller.isGrounded;
        Func<bool> Grounded() => () => currentCharacter.controller.isGrounded;
        Func<bool> Slowed() => () => isSlowed;
        Func<bool> NotSlowed() => () => !isSlowed;
        Func<bool> Vaulting() => () => IsVaulting && CheckForVaultableObject() && CheckForwardMovement();
        Func<bool> NotVaulting() => () => !IsVaulting || !CheckForwardMovement();

        // Sub State Conditions
        Func<bool> Walked() => () => inputManager.moveAction.triggered && !inputManager.runAction.triggered;
        Func<bool> Stopped() => () => inputManager.moveAction.ReadValue<Vector2>().magnitude == 0;
        Func<bool> Running() => () => inputManager.moveAction.triggered && inputManager.runAction.triggered;

        stateMachine.SetState(standardState);
    }
    void OnDisable()
    {
        inputManager.OnSlowActionStateChanged -= SlowMotion;
        inputManager.OnSwapPressed -= SwapPressed;
    }
    void Update()
    {
        stateMachine.Tick();
    }
    void FixedUpdate()
    {
        currentCharacter.controller.Move(appliedMovement * Time.deltaTime);
    }

    private void SwapPressed()
    {
        SwapCharacter(CheckForCharacter());
    }
    public GameObject CheckForCharacter()
    {
        // Create a BoxCast to check for objects with the "Swapable" tag.
        Vector3 boxCenter = Camera.main.transform.position + Camera.main.transform.forward * (swapDistance / 2f);
        Vector3 boxHalfExtents = new Vector3(0.5f, 0.5f, swapDistance / 2f);
        Quaternion boxRotation = Camera.main.transform.rotation;

        // Store the results of the BoxCast.
        RaycastHit[] hits = Physics.BoxCastAll(boxCenter, boxHalfExtents, Vector3.forward, boxRotation, swapDistance, swapableLayer);

        // Loop through the hits to find the first object with the "Swapable" tag.
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Swapable") && hit.collider.gameObject != currentCharacter.characterContainer)
            {
                // Return the GameObject that was hit.
                return hit.collider.gameObject;
            }
        }

        // If no "Swapable" object was hit, return null.
        return null;
    }
    public void SwapCharacter(GameObject newCharacter)
    {
        if(newCharacter == null) return;
        isSlowed = false;
        Debug.Log(currentCharacter);
        currentCharacter = new CharacterInfo
        {
            characterContainer = newCharacter,
            model = newCharacter.transform.GetChild(1).gameObject,
            cameraRoot = newCharacter.transform.GetChild(0),
            controller = newCharacter.GetComponent<CharacterController>()
        };
        virtualCamera.Follow = currentCharacter.cameraRoot;
        Debug.Log(currentCharacter);
    }   

    private void SetJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        initialJumpGravity = -2 * maxJumpHeight / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = 2 * maxJumpHeight / timeToApex;
    } 

    private void SlowMotion(bool timeSlow)
    {
        isSlowed = timeSlow;
    }
    public void SwapControlMap(bool slow)
    {
        if(slow)
            inputManager.playerInput.SwitchCurrentActionMap(inputManager.slowActionMap.name);
        else
            inputManager.playerInput.SwitchCurrentActionMap(inputManager.standardActionMap.name);
    }

    private bool CheckForVaultableObject()
    {
        float boxWidth = 0.5f;
        float boxHeight = 1.0f;
        float boxDepth = 0.5f;
        Vector3 boxCenter = currentCharacter.model.transform.position + Vector3.up * (boxHeight / 2) + Vector3.up * (boxHeight / 2 - boxDepth / 2);
        bool isHit = Physics.BoxCast(boxCenter, new Vector3(boxWidth / 2, boxHeight / 2, boxDepth / 2), currentCharacter.model.transform.forward, out RaycastHit hitInfo, currentCharacter.model.transform.rotation, 1f, VaultLayers);
        return isHit;
    }
    private bool CheckForwardMovement()
    {
        Vector2 input = inputManager.moveAction.ReadValue<Vector2>();
        Vector3 forward = currentCharacter.model.transform.forward;

        float dotProduct = Vector3.Dot(forward, new Vector3(input.x, 0, input.y));

        return dotProduct > 0.75f;
    }
}
