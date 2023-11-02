using System;
using UnityEngine;
using Cinemachine;
//using System.Threading.Tasks;
using Unity.Mathematics;
using Unity.VisualScripting;
using System.Linq;
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
        public Transform cameraTransform;
        #endregion

        #region Movement Variables
        [Header("Movement Variables")]
        private Vector3 currentMovement;
        private Vector3 appliedMovement;
        public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; }}
        //public float CurrentMovementX { get { return currentMovement.x; } set { currentMovement.x = value; }}
        public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; }}
        public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; }}
        public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; }}
        //public float CurrentMovementZ { get { return currentMovement.z; } set { currentMovement.z = value; }}

        public Vector3 InAirForward { get; set; }
        public Vector3 InAirRight { get; set; }
        [SerializeField] private float walkSpeed = 20;
        public float WalkSpeed { get { return walkSpeed; } }
        public float smoothInputSpeed;

        public Vector2 currentInputVector { get; set; }
        [HideInInspector] public Vector2 smoothInputVelocity;
        public Vector3 move { get; set ; }

        [Header("Ground Variables")]
        [SerializeField] private LayerMask groundLayer;

        #region  Wall
        [Header("Wall Variables")]
        [SerializeField] private float wallSpeed;
        public float WallSpeed { get { return wallSpeed;  } }
        public LayerMask wallLayer;
        [SerializeField] private LayerMask vaultLayers;
        [SerializeField] private LayerMask wallholdLayers;
        public LayerMask VaultLayers { get { return vaultLayers; } }
        public bool IsVaulting { get; set; }
        public bool StaticMode { get; set; }



        [SerializeField] float wallCheckDistance;
        [SerializeField] float minJumpHeight;

        #endregion

        [Header("Jump Variables")]
        [SerializeField] private float jumpForce = 1f;
        public float JumpForce { get { return jumpForce; } }
        
        public float gravityValue = -9.81f;
        [SerializeField] float maxJumpHeight = 4.0f;
        public float maxJumpTime = 0.75f;
        private float initialJumpVelocity;
        private float initialJumpGravity;
        public float InitialJumpVelocity { get { return initialJumpVelocity; } }
        public float InitialJumpGravity { get { return initialJumpGravity; } } 
        [SerializeField] private float maxFallSpeed = 30f;
        public float MaxFallSpeed { get { return maxFallSpeed; }}
        #endregion
        #region Manipulate Variables
        [Header("Manipulation Variables")]
        [SerializeField] private LayerMask manipulateLayers;
        #endregion
        #region Swap Variables
        [Header("Swapping Variables")]
        [SerializeField] public float swapDistance = 100f;
        public float SwapDistance { get { return swapDistance; } set { swapDistance = value; } }
        [SerializeField] private LayerMask swapableLayer;
        public LayerMask SwapableLayer { get { return swapableLayer; }}
        // Create a BoxCast to check for objects with the "Swapable" tag.
        Vector3 boxHalfExtents = new Vector3(2f, 2f, 2f);
        Quaternion boxRotation;
        #endregion
        
        #region  Slow Variables
        private bool isSlowed = false;
        #endregion
        [SerializeField] private GameObject tempCharacter;
        public CharacterInfo currentCharacter { get; set; }
        #region Monobehaviours
        private void Awake()
        {
            WallStateVariables.Instance.wallLayer = wallLayer;
            WallStateVariables.Instance.WallholdLayers = wallholdLayers;
            WallStateVariables.Instance.WallSpeed = wallSpeed;
            boxRotation = Camera.main.transform.rotation;
            SetJumpVariables();
            cameraTransform = Camera.main.transform;
            cameraTransform.GetComponent<CinemachineBrain>().m_IgnoreTimeScale = false;
            SwapCharacter(tempCharacter);

            virtualCamera.Follow = currentCharacter.cameraRoot;

            
            stateMachine = new StateMachine.StateMachine();

            #region Function Events
            InputManager.Instance.OnSlowActionStateChanged += SlowMotion;
            InputManager.Instance.OnSwapPressed += SwapPressed;
            InputManager.Instance.OnManipulatePressed += Manipulate;
            InputManager.Instance.OnSwitchPressed += SwitchMode;
            #endregion

            // Create instances of root states
            var groundState = new GroundedState(this, stateMachine);
            var fallState = new FallState(this, stateMachine);
            var jumpState = new JumpState(this, stateMachine);
            var wallFlowState = new WallFlowState(this, stateMachine);
            var wallStaticState = new WallStaticState(this, stateMachine);
            var wallJumpState = new WallJumpState(this, stateMachine);

            // Create instances of sub-states
            var idleState = new IdleState(this);
            var walkState = new WalkState(this);

            var wallRunState = new WallRunState(this, this, virtualCamera.GetComponent<CinemachineTiltExtension>());
            var vaultState = new VaultState(this);
            var mantleState = new MantleState(this);
            var wallHangState = new WallHangState(this);

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
            AT(jumpState, wallFlowState, OnWallFlow());
            AT(jumpState, wallStaticState, OnWallStatic());
            // Leave Fall State
            AT(fallState, groundState, Grounded());    
            AT(fallState, wallFlowState, OnWallFlow());  
            AT(fallState, wallStaticState, OnWallStatic());  
            // Leave Wall Flow State
            AT(wallFlowState, groundState, Grounded());
            AT(wallFlowState, fallState, NotOnWallFlow());
            AT(wallFlowState, wallJumpState, WallJump());
            AT(wallFlowState, wallStaticState, OnWallStatic());
            // Leave Wall Static State
            AT(wallStaticState, groundState, Grounded());
            AT(wallStaticState, fallState, NotOnWallStatic());
            AT(wallStaticState, wallJumpState, WallJump());
            AT(wallStaticState, wallFlowState, OnWallFlow());
            // Leave Wall Jump State
            AT(wallJumpState, fallState, WallFall());
            AT(wallJumpState, groundState, Grounded());
            #endregion

            #region Standard Transitions
            AT(idleState, walkState, Walked());
            AT(walkState, idleState, Stopped());
            #endregion
            #region Wall Sub State Transitions
            AT(wallRunState, idleState, Stopped());
            AT(idleState, wallRunState, WallRun());

            AT(wallHangState, vaultState, ClimbUpLedge());
            AT(vaultState, wallRunState, WallRun());
            
            #endregion

            #region Assign Substates to Rootstates
            groundState.AddSubState(idleState);
            groundState.AddSubState(walkState);
            groundState.PrepareSubStates();
            groundState.SetDefaultSubState(idleState);

            jumpState.AddSubState(idleState);
            jumpState.AddSubState(walkState);
            jumpState.PrepareSubStates();
            jumpState.SetDefaultSubState(idleState);

            fallState.AddSubState(idleState);
            fallState.AddSubState(walkState);
            fallState.PrepareSubStates();
            fallState.SetDefaultSubState(idleState);

            wallFlowState.AddSubState(wallRunState);
            wallFlowState.AddSubState(idleState);
            wallFlowState.PrepareSubStates();
            wallFlowState.SetDefaultSubState(wallRunState);

            wallStaticState.AddSubState(wallHangState);
            wallStaticState.AddSubState(vaultState);
            //wallStaticState.AddSubState(idleState);
            wallStaticState.PrepareSubStates();
            wallStaticState.SetDefaultSubState(wallHangState);

            wallJumpState.AddSubState(idleState);
            wallJumpState.AddSubState(walkState);
            wallJumpState.PrepareSubStates();
            wallJumpState.SetDefaultSubState(idleState);
            #endregion

            // Root State Conditions
            Func<bool> Jumped() => () => InputManager.Instance.IsJumpPressed && GroundedCheck();
            Func<bool> Falling() => () => AppliedMovementY < 0 && !GroundedCheck();
            Func<bool> Grounded() => () => GroundedCheck();
            Func<bool> OnWallStatic() => () => CheckForWall() && AboveGround() && StaticMode;
            Func<bool> OnWallFlow() => () => CheckForWall() && AboveGround() && InputManager.Instance.GetPlayerMovement().magnitude > 0 && !StaticMode;
            Func<bool> NotOnWallFlow() => () => !WallStateVariables.Instance.CheckOnWall() || InputManager.Instance.GetPlayerMovement().magnitude == 0 || StaticMode;
            Func<bool> NotOnWallStatic() => () => !WallStateVariables.Instance.CheckOnWall() || !StaticMode;
            Func<bool> WallJump() => () => InputManager.Instance.IsJumpPressed && WallStateVariables.Instance.TimeOnWall > 0.4f;
            Func<bool> WallFall() => () => AppliedMovementY < 0 && !GroundedCheck() && WallStateVariables.Instance.TimeOffWall > 0.2f;

            // Sub State Conditions
            Func<bool> Walked() => () => InputManager.Instance.GetPlayerMovement().magnitude != 0;
            Func<bool> Stopped() => () => InputManager.Instance.GetPlayerMovement().magnitude == 0;

            //Func<bool> ForwardWall() => () => WallStateVariables.Instance.ForwardWall && InputManager.Instance.GetPlayerMovement().magnitude == 0;
            Func<bool> WallRun() => () => WallStateVariables.Instance.WallRight || WallStateVariables.Instance.WallLeft;
            Func<bool> Ledge() => () => WallStateVariables.Instance.LedgeDetection(currentCharacter, cameraTransform) && WallStateVariables.Instance.ForwardWall && StaticMode;
            Func<bool> ClimbUpLedge() => () => WallStateVariables.Instance.ForwardWall && InputManager.Instance.IsJumpPressed && StaticMode;

            stateMachine.SetState(groundState);
        }
        void OnDisable()
        {
            InputManager.Instance.OnSlowActionStateChanged -= SlowMotion;
            InputManager.Instance.OnSwapPressed -= SwapPressed;
        }
        void Update()
        {
            RotatePlayer();
            IsVaulting = CheckForVaultableObject();
            stateMachine.Tick();
            if (isSlowed)
                SearchForInteractable();
        }
        void FixedUpdate()
        {
            currentCharacter.controller.Move(appliedMovement * Time.deltaTime);
        }
        #endregion

        #region Swapping
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
            SlowMotion(false);
            if(currentCharacter != null)
            {
                currentCharacter.characterContainer.GetComponent<CharacterInfoReference>().DeactivateCharacter();
            } 

            CharacterInfoReference newCharacterInfoReference = newCharacter.GetComponent<CharacterInfoReference>();
            newCharacterInfoReference.ActivateCharacter();

            currentCharacter = newCharacterInfoReference.characterInfo;
            virtualCamera.Follow = currentCharacter.cameraRoot;

            /* 
            FIND A WAY TO MAKE THE CAMERA LOOK IN THE DIRECTINO THE NEW BODY IS LOOKING

            virtualCamera.enabled = false;
            cameraTransform.rotation = currentCharacter.model.transform.rotation;
            virtualCamera.enabled = true; */
        }
        #endregion

        
        private void SetJumpVariables()
        {
            float timeToApex = maxJumpTime / 2;
            initialJumpGravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
            initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        }
        
        #region Time Management
        private void SlowMotion(bool timeSlow)
        {
            if(timeSlow)
            {
                InputManager.Instance.SwapControlMap(ActionMapEnum.slow);
                TimeManager.Instance.DoSlowmotion(0.1f);
            }
            else
            {
                InputManager.Instance.SwapControlMap(ActionMapEnum.standard);
                TimeManager.Instance.UndoSlowmotion();
            }
            isSlowed = timeSlow;
        }
        private Outliner currentOutlinedObject;

        private void SearchForInteractable()
        {
            GameObject hitObject = CheckForCharacter();

            // Check if the hit GameObject has the specified script component
            if (hitObject == null)
            {
                // Player is not looking at any object, deactivate current outline (if any)
                if (currentOutlinedObject != null)
                {
                    currentOutlinedObject.ActivateOutline(false);
                    currentOutlinedObject = null;
                }
                return;
            }

            Outliner outliner = hitObject.GetComponent<Outliner>();

            if (outliner == null)
            {
                // Player is looking at an object without an Outliner component, deactivate current outline (if any)
                if (currentOutlinedObject != null)
                {
                    currentOutlinedObject.ActivateOutline(false);
                    currentOutlinedObject = null;
                }
                return;
            }

            // Player is looking at an object with an Outliner component
            if (currentOutlinedObject != null && currentOutlinedObject != outliner)
            {
                // Deactivate the outline of the previously outlined object
                currentOutlinedObject.ActivateOutline(false);
            }

            // Activate the outline of the currently looked at object
            currentOutlinedObject = outliner;
            currentOutlinedObject.ActivateOutline(true);
        }

        #endregion


        public virtual void RotatePlayer()
        {
            Vector2 mouseDelta = InputManager.Instance.GetMouseDelta();
            Vector3 currentRotation = cameraTransform.localRotation.eulerAngles;

            currentRotation.x -= mouseDelta.y;
            currentRotation.y += mouseDelta.x;

            currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);

            cameraTransform.localRotation = Quaternion.Euler(currentRotation);
            currentCharacter.characterContainer.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);

        }
        private bool CheckForVaultableObject()
        {
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var firstHit, 1f, vaultLayers))
                return true;
            return false;
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

        #region Wall
        Vector3[] wallCheckDirections = new Vector3[] { Vector3.forward, Vector3.right, Vector3.left, (Vector3.left + Vector3.forward), (Vector3.right + Vector3.forward) };
        private bool CheckForWall()
        {
            foreach(Vector3 dir in wallCheckDirections)
            {
                Vector3 relativeDir = currentCharacter.model.transform.TransformDirection(dir);
                Debug.DrawRay(currentCharacter.wallCheck.position, relativeDir, Color.blue, 0);
                if (Physics.Raycast(currentCharacter.wallCheck.position, relativeDir, 2.5f, wallLayer))
                    return true;
            }
            return false;
        }
        #endregion

        #region Ground Check

        Vector3 castDirection = Vector3.down;
        float castDistance = 0.0f;
        Vector3 boxSize = new Vector3(0.4f, 0.1f, 0.4f);
        private bool GroundedCheck()
        {
            RaycastHit[] hits = Physics.BoxCastAll(currentCharacter.characterContainer.transform.position, boxSize, castDirection, Quaternion.identity, castDistance, groundLayer);
            if(hits.Any(hit => hit.collider != null))
                return true;
            
            return false;
        }

        private bool AboveGround()
        {
            RaycastHit[] hits = Physics.BoxCastAll(currentCharacter.characterContainer.transform.position, boxSize, castDirection, Quaternion.identity, 0.2f, groundLayer);
            if(hits.Any(hit => hit.collider != null))
                return false;
            
            return true;
        }
        #endregion

        private void SwitchMode(bool staticMode)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift) && StaticBar.instance.currentStatic >= 1)
            {

                StaticMode = staticMode;
            }
            //Debug.Log("Static Mode == " + StaticMode);
            else
            {
            StaticMode = !staticMode;
            }
            if(Input.GetKeyUp(KeyCode.LeftShift))
            {
            StaticMode = !staticMode;
            }
        }

        private void OnDrawGizmos()
        {
            if(Application.isPlaying)
            {
                bool isHit;
                if(currentCharacter != null)
                {
                    RaycastHit[] hits = Physics.BoxCastAll(currentCharacter.characterContainer.transform.position, boxSize, castDirection, Quaternion.identity, castDistance, groundLayer);
                    if(hits.Any(hit => hit.collider != null))
                    {
                        Gizmos.color = Color.green;
                        Gizmos.DrawWireCube(currentCharacter.characterContainer.transform.position, boxSize);
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawWireCube(currentCharacter.characterContainer.transform.position, boxSize);
                    }

                }
                isHit = Physics.BoxCast(cameraTransform.position, boxHalfExtents, cameraTransform.forward, out RaycastHit hit, boxRotation, swapDistance, swapableLayer);
                if (isHit)
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
}
