using System;
using UnityEngine;
using Cinemachine;
namespace Player
{
    [RequireComponent(typeof(InputManager))]
    public class PlayerStateMachine : MonoBehaviour
    {
        #region References
        [Header("Look Speeds")]
        //[SerializeField] private float vertLookSpeed = 300, horzLookSpeed = 300, slowVertLookSpeed = 150, slowHorzLookSpeed = 150;
        public CinemachineVirtualCamera virtualCamera;
        private StateMachine.StateMachine stateMachine;
        private InputManager inputManager;
        public Transform cameraTransform;
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
        public float gravityValue = -9.81f;
        [SerializeField] float maxJumpHeight = 4.0f;
        [SerializeField] float maxJumpTime = 0.75f;
        private float initialJumpVelocity;
        private float initialJumpGravity;
        public float InitialJumpVelocity { get { return initialJumpVelocity; } }
        public float InitialJumpGravity { get { return initialJumpGravity; } }
        #endregion
        #region Manipulate Variables
        [SerializeField] private LayerMask manipulateLayers;
        #endregion
        #region Swap Variables
        [SerializeField] public float swapDistance = 100f;
        public float SwapDistance { get { return swapDistance; } set { swapDistance = value; } }
        [SerializeField] private LayerMask swapableLayer;
        public LayerMask SwapableLayer { get { return swapableLayer; }}
        // Create a BoxCast to check for objects with the "Swapable" tag.
        Vector3 boxHalfExtents;
        Quaternion boxRotation;
        #endregion
        
        #region  Slow Variables
        private bool isSlowed = false;
        #endregion
        [SerializeField] private GameObject tempCharacter;
        public CharacterInfo currentCharacter { get; set; }
        private void Awake()
        {
            boxHalfExtents = new Vector3(2f, 2f, 2f);
            boxRotation = Camera.main.transform.rotation;
            SetJumpVariables();
            cameraTransform = Camera.main.transform;
            cameraTransform.GetComponent<CinemachineBrain>().m_IgnoreTimeScale = false;
            SwapCharacter(tempCharacter);
            virtualCamera.Follow = currentCharacter.cameraRoot;
            inputManager = GetComponent<InputManager>();
            stateMachine = new StateMachine.StateMachine();

            #region Function Events
            inputManager.OnSlowActionStateChanged += SlowMotion;
            inputManager.OnSwapPressed += SwapPressed;
            inputManager.OnManipulatePressed += Manipulate;
            #endregion

            // Create instances of root states
            var slowState = new SlowState(this, stateMachine);
            var standardState = new StandardState(this, stateMachine);

            var groundState = new GroundedState(this, stateMachine);
            var fallState = new FallState(this, stateMachine);
            var jumpState = new JumpState(this, stateMachine);
            //var vaultState = new VaultState(this);

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
            //AT(groundState, vaultState, Vaulting());
            // Leave Jump State
            AT(jumpState, groundState, Grounded());
            AT(jumpState, fallState, Falling());
            //AT(jumpState, vaultState, Vaulting());
            // Leave Fall State
            AT(fallState, groundState, Grounded());
            //AT(fallState, vaultState, Vaulting());
            // Leave Vault State
            //AT(vaultState, groundState, NotVaulting());
            //AT(vaultState, fallState, NotVaulting());
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
            //standardState.AddSubState(vaultState);
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
            Func<bool> Jumped() => () => inputManager.IsJumpPressed && currentCharacter.controller.isGrounded;
            Func<bool> Falling() => () => AppliedMovementY < 0 && !currentCharacter.controller.isGrounded;
            Func<bool> Grounded() => () => currentCharacter.controller.isGrounded;
            Func<bool> Slowed() => () => isSlowed;
            Func<bool> NotSlowed() => () => !isSlowed;
            /* Func<bool> Vaulting() => () => IsVaulting && CheckForwardMovement();
            Func<bool> NotVaulting() => () => !IsVaulting || !CheckForwardMovement(); */

            // Sub State Conditions
            Func<bool> Walked() => () => inputManager.MoveAction.triggered && !inputManager.RunAction.triggered;
            Func<bool> Stopped() => () => inputManager.MoveAction.ReadValue<Vector2>().magnitude == 0;
            Func<bool> Running() => () => inputManager.MoveAction.triggered && inputManager.RunAction.triggered;

            stateMachine.SetState(standardState);
        }
        void OnDisable()
        {
            inputManager.OnSlowActionStateChanged -= SlowMotion;
            inputManager.OnSwapPressed -= SwapPressed;
        }
        void Update()
        {
            IsVaulting = CheckForVaultableObject();
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
            

            // Store the results of the BoxCast.
            RaycastHit[] hits = Physics.BoxCastAll(cameraTransform.position, boxHalfExtents, cameraTransform.forward, boxRotation, swapDistance, swapableLayer);
            Debug.DrawRay(cameraTransform.position, Camera.main.transform.forward * swapDistance, Color.green, 0.5f);
            // Loop through the hits to find the first object with the "Swapable" tag.
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Swapable") && hit.collider.gameObject != currentCharacter.characterContainer)
                {
                    // Return the GameObject that was hit if there is line of sight.
                    RaycastHit lineOfSightHit;
                    if (Physics.Raycast(cameraTransform.position, (hit.point - cameraTransform.position).normalized, out lineOfSightHit, swapDistance))
                    {
                        if (lineOfSightHit.collider.gameObject == hit.collider.gameObject)
                        {
                            return hit.collider.gameObject;
                        }
                    }
                }
            }
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.CompareTag("Manipulatable"))
                {
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
            if(currentCharacter != null) currentCharacter.model.GetComponent<ModelDisplay>().DeActivateFirstPerson();
            currentCharacter = new CharacterInfo
            {
                characterContainer = newCharacter,
                cameraRoot = newCharacter.transform.GetChild(0),
                model = newCharacter.transform.GetChild(1).gameObject,
                controller = newCharacter.GetComponent<CharacterController>()
            };
            Debug.Log(currentCharacter.model);
            currentCharacter.model.GetComponent<ModelDisplay>().ActivateFirstPerson();
        
            virtualCamera.Follow = currentCharacter.cameraRoot;
        }

        private void SetJumpVariables()
        {
            float timeToApex = maxJumpTime / 2;
            initialJumpGravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        } 

        private void SlowMotion(bool timeSlow)
        {
            isSlowed = timeSlow;
        }
        

        private bool CheckForVaultableObject()
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var firstHit, 1f, vaultLayers))
                return true;
            return false;
        }
        private bool CheckForwardMovement()
        {
            Vector2 input = inputManager.MoveAction.ReadValue<Vector2>();
            Vector3 forward = currentCharacter.model.transform.forward;

            float dotProduct = Vector3.Dot(forward, new Vector3(input.x, 0, input.y));

            return dotProduct > 0.75f;
        }

        private void Manipulate()
        {
            // Check if player has suffecient brain juice for action here.



            // Create a BoxCast to check for objects with the "Swapable" tag.
            Vector3 boxHalfExtents = new Vector3(0.5f, 0.5f, swapDistance / 2f);
            Quaternion boxRotation = Camera.main.transform.rotation;

            // Store the results of the BoxCast.
            RaycastHit[] hits = Physics.BoxCastAll(cameraTransform.position, boxHalfExtents, Vector3.forward, boxRotation, swapDistance, manipulateLayers);

            // Loop through the hits to find the first object with the "Swapable" tag.
            foreach (RaycastHit hit in hits)
            {
                IManipulate manipulateScript = hit.collider.gameObject.GetComponent<IManipulate>();
                if (manipulateScript != null)
                {
                    manipulateScript.Interacted();
                    return;
                }
            }
        }

        private void OnDrawGizmos()
        {
            RaycastHit hit;

            bool isHit = Physics.BoxCast(cameraTransform.position, boxHalfExtents, cameraTransform.forward, out hit, boxRotation, swapDistance, swapableLayer);
            if(isHit)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * hit.distance);
                Gizmos.DrawWireCube(cameraTransform.position + cameraTransform.forward * hit.distance, boxHalfExtents);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(cameraTransform.position, cameraTransform.forward * swapDistance);
            }
        }
    }
}
