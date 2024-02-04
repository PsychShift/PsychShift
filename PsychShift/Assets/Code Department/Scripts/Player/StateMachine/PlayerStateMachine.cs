using System;
using UnityEngine;
using Cinemachine;
using System.Linq;
using System.Collections;
using Guns.Demo;

namespace Player
{
    [RequireComponent(typeof(InputManager))]
    public class PlayerStateMachine : MonoBehaviour
    {
        private static PlayerStateMachine instance;
        public static PlayerStateMachine Instance { get { return instance; } }
        //AUDIO SOURCE HERE
        [SerializeField] public AudioSource playerAudio;
        #region References
        [Header("Look Speeds")]
        //[SerializeField] private float vertLookSpeed = 300, horzLookSpeed = 300, slowVertLookSpeed = 150, slowHorzLookSpeed = 150;
        //public CinemachineVirtualCamera virtualCamera;
        private StateMachine.StateMachine stateMachine;
        [SerializeField] private PlayerGunSelector gunSelector;
        public Transform cameraTransform;
        private ParticleMaster particleMaster;
        [SerializeField] private HealthUI healthUI;
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

        public Vector3 ExternalMovement { get; set; }

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
        [SerializeField] private float wallJumpForce = 25f;
        //KEVIN ADDED THIS FOR THE WALL STATE
        [SerializeField] public GameObject wallRunEffect;
        [SerializeField] public AudioClip wallRunSound;
        
        [SerializeField] float wallJumpAngle = 0.7f;
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
        public delegate void SwapEvent(Transform player);
        public event SwapEvent OnSwapPlayer;
        //mindswap fx and arms
        private ParticleSystem mindSwapTunnel;
        [SerializeField] GameObject armsOff;
        [SerializeField] AudioClip mindswapBegin;
        bool playingBeginning;
        [SerializeField] AudioClip mindswapDuring;
        bool playingDuring;
        [SerializeField] AudioClip mindswapEnd;
        bool playingEnd;
        #endregion
        
        #region  Slow Variables
        private bool isSlowed = false;
        #endregion
        [SerializeField] private GameObject tempCharacter;
        public CharacterInfo currentCharacter;
        private static Vector3 checkPointL;
        #region Monobehaviours
        public HitEffects hitMarkerSRef;
        private void Awake()
        {
            if(instance != null)
            {
                Debug.LogError("More than one PlayerStateMachine active in the scene! Destroying latest one: " + name);
                Destroy(gameObject);
                return;
            }
            instance = this;
            WallStateVariables.Instance.wallLayer = wallLayer;
            WallStateVariables.Instance.WallholdLayers = wallholdLayers;
            WallStateVariables.Instance.WallSpeed = wallSpeed;
            boxRotation = Camera.main.transform.rotation;
            SetJumpVariables();
            cameraTransform = Camera.main.transform;
            cameraTransform.GetComponent<CinemachineBrain>().m_IgnoreTimeScale = false;
            particleMaster = GetComponentInChildren<ParticleMaster>();

            OnSwapPlayer += EnemyTargetManager.Instance.SetPlayer;

            
            currentCharacter = null;
            SwapCharacter(tempCharacter, PlayerMaster.Instance.checkPointLocation);
            // This is the first character, if it has a the FirstEnemy script, subrsribe its Swap function to the OnSwapPlayer event.
            if(currentCharacter != null)
            {
                if(currentCharacter.characterContainer.TryGetComponent(out FirstEnemy firstEnemy))
                {
                    firstEnemy.playerStateMachine = this;
                    OnSwapPlayer += firstEnemy.Swap;
                }
            }

            //virtualCamera.Follow = currentCharacter.cameraRoot;

            
            stateMachine = new StateMachine.StateMachine();

            #region Function Events
            InputManager.Instance.OnSlowActionStateChanged += SlowMotion;
            InputManager.Instance.OnSwapPressed += SwapPressed;
            InputManager.Instance.OnManipulatePressed += Manipulate;
            InputManager.Instance.OnSwitchPressed += SwitchMode;
            #endregion

            // Create instances of root states
            /* var groundState = new GroundedState(this, stateMachine);
            var fallState = new FallState(this, stateMachine);
            var jumpState = new JumpState(this, stateMachine);
            var wallFlowState = new WallFlowState(this, stateMachine);
            var wallStaticState = new WallStaticState(this, stateMachine);
            var wallJumpState = new WallJumpState(this, stateMachine); */
            var groundState = new GroundedState(this);
            var fallState = new FallState(this);
            var jumpState = new JumpState(this);
            var wallFlowState = new WallFlowState(this);
            var wallStaticState = new WallStaticState(this);
            var wallJumpState = new WallJumpState(this, wallJumpForce, wallJumpAngle);
            var wallFallState = new WallFallState(this, wallJumpForce, wallJumpAngle);

           /*  // Create instances of sub-states
            var idleState = new IdleState(this);
            var walkState = new WalkState(this);

            var wallRunState = new WallRunState(this, this);
            var vaultState = new VaultState(this);
            var mantleState = new MantleState(this);
            var wallHangState = new WallHangState(this); */

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
            AT(wallFlowState, fallState, Swapped());
            // Leave Wall Static State
            AT(wallStaticState, groundState, Grounded());
            AT(wallStaticState, fallState, NotOnWallStatic());
            AT(wallStaticState, wallJumpState, WallJump());
            AT(wallStaticState, wallFlowState, OnWallFlow());
            // Leave Wall Jump State
            AT(wallJumpState, wallFallState, WallFall());
            AT(wallJumpState, groundState, Grounded());
            // Leave Wall Fall State
            AT(wallFallState, groundState, Grounded());    
            AT(wallFallState, wallFlowState, OnWallFlow());  
            AT(wallFallState, wallStaticState, OnWallStatic());
            AT(wallFallState, fallState, wallFallState.IsDone());
            #endregion

            /* #region Standard Transitions
            AT(idleState, walkState, Walked());
            AT(walkState, idleState, Stopped());
            #endregion
            #region Wall Sub State Transitions
            AT(wallRunState, idleState, Stopped());
            AT(idleState, wallRunState, WallRun());

            AT(wallHangState, vaultState, ClimbUpLedge());
            AT(vaultState, wallRunState, WallRun());
            
            #endregion */

            #region Assign Substates to Rootstates
            /* groundState.AddSubState(idleState);
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
            wallJumpState.SetDefaultSubState(idleState); */
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
            Func<bool> Swapped() => () => isSwapping;

            /* // Sub State Conditions
            Func<bool> Walked() => () => InputManager.Instance.GetPlayerMovement().magnitude != 0;
            Func<bool> Stopped() => () => InputManager.Instance.GetPlayerMovement().magnitude == 0;

            //Func<bool> ForwardWall() => () => WallStateVariables.Instance.ForwardWall && InputManager.Instance.GetPlayerMovement().magnitude == 0;
            Func<bool> WallRun() => () => WallStateVariables.Instance.WallRight || WallStateVariables.Instance.WallLeft;
            //Func<bool> Ledge() => () => WallStateVariables.Instance.LedgeDetection(currentCharacter, cameraTransform) && WallStateVariables.Instance.ForwardWall && StaticMode;
            Func<bool> ClimbUpLedge() => () => WallStateVariables.Instance.ForwardWall && InputManager.Instance.IsJumpPressed && StaticMode; */

            stateMachine.SetState(groundState);
        }
        void OnDisable()
        {
            currentCharacter.enemyHealth.OnTakeDamage -= healthUI.UpdateHealthBar;
            currentCharacter.enemyHealth.OnDeath -= healthUI.HandleDeath;
            OnSwapPlayer -= EnemyTargetManager.Instance.SetPlayer;
            InputManager.Instance.OnSlowActionStateChanged -= SlowMotion;
            InputManager.Instance.OnSwapPressed -= SwapPressed;
            InputManager.Instance.OnManipulatePressed -= Manipulate;
            InputManager.Instance.OnSwitchPressed -= SwitchMode;
        }
        void Update()
        {
            RotatePlayer();
            //IsVaulting = CheckForVaultableObject();
            stateMachine.Tick();
            if (isSlowed)
                SearchForInteractable();

            currentCharacter.animator.SetFloat("speed", 0);
        }
        void FixedUpdate()
        {
            currentCharacter.controller.Move((appliedMovement * Time.deltaTime));
        }
        #endregion

        #region Swapping
        private void SwapPressed()
        {
            SwapCharacter(CheckForCharacter());
        }
        public GameObject CheckForCharacter()
        {
            // Perform a BoxCast to check for objects in the specified direction.
            RaycastHit hit;
            if (Physics.BoxCast(cameraTransform.position, boxHalfExtents, cameraTransform.forward, out hit, boxRotation, swapDistance, swapableLayer))
            {
                if(hit.collider == null) return null;
                if(!IsObjectVisible(Camera.main, hit.collider.gameObject, hit)) return null;

                if (hit.collider.CompareTag("Swapable") && hit.collider.gameObject != currentCharacter.characterContainer)
                {
                    // No need for the second raycast, as BoxCast already checks for obstacles.
                    return hit.collider.gameObject;
                }
                else if (hit.collider.CompareTag("Manipulatable"))
                {
                    return hit.collider.gameObject;
                }
            }

            // Return null if no valid object is found.
            return null;
        }

        public bool IsObjectVisible(Camera cam, GameObject obj, RaycastHit firstHit)
        {
            // Calculate the direction from the camera to the object
            Vector3 dir = firstHit.point - cam.transform.position;
            dir.Normalize();
            // Perform a raycast
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position + dir, dir, out hit))
            {
                // If the raycast hits the object, the object is potentially visible
                return hit.collider.gameObject == obj;
            }
            
            // If the raycast didn't hit anything, the object is not visible
            return false;
        }
        /* public bool IsObjectVisible(GameObject obj)
        {
            Renderer renderer = FindRendererInChildren(obj.transform);
            if(renderer == null) return false;
            
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
            return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
        }
        private Renderer FindRendererInChildren(Transform parent)
        {
            Renderer renderer = parent.GetComponent<Renderer>();
            if (renderer != null)
            {
                return renderer;
            }

            foreach (Transform child in parent)
            {
                renderer = FindRendererInChildren(child);
                if (renderer != null)
                {
                    return renderer;
                }
            }

            return null;
        } */
        public void SwapCharacter(GameObject newCharacter)
        {
            
            if(newCharacter == null) return;
            CharacterInfoReference newCharacterInfoReference = newCharacter.GetComponent<CharacterInfoReference>();
            CharacterInfo newCharInfo = newCharacterInfoReference.characterInfo;
            SlowMotion(false);
            
            if(currentCharacter != null)
            {
                if (BrainJuiceBarTest.instance.currentBrain >= 15)//This is where mindswap starts
                {
                    CharacterInfoReference oldCharacterInfoReference = currentCharacter.characterContainer.GetComponent<CharacterInfoReference>();
                    StartCoroutine(SwapAnimation(currentCharacter.cameraRoot.transform, 
                    newCharacterInfoReference.characterInfo.cameraRoot.transform, 
                    oldCharacterInfoReference, newCharacterInfoReference));
                    BrainJuiceBarTest.instance.UseBrain(15);
                    /* if(playingBeginning ==false)
                    {
                        playerAudio.PlayOneShot(mindswapBegin);
                        playingBeginning = true;
                    } */
                    playerAudio.PlayOneShot(mindswapBegin);
                    armsOff.SetActive(false);

                    // Subscribe the hit effects to the new gun, unsubscribe the old gun
                    // FYI, this doesn't work right yet, I need to make it do this for the current gun attached to the camera, not the enemy
                    currentCharacter.gunHandler.ActiveGun.OnSomethingHit -= HitDamageable;
                    newCharInfo.gunHandler.ActiveGun.OnSomethingHit += HitDamageable;

                    currentCharacter = newCharInfo;
                }
            }
            else
            {
                currentCharacter = newCharInfo;
                newCharacterInfoReference.vCamParent.SetActive(true);
                newCharacterInfoReference.ActivatePlayerAllAtOnce();
                OnSwapPlayer?.Invoke(currentCharacter.characterContainer.transform);
                gunSelector.SetupGun(currentCharacter.gunHandler.StartGun);
                // Setup healthbar ui
                healthUI.SetHealthBarOnSwap(currentCharacter.enemyHealth.CurrentHealth, currentCharacter.enemyHealth.MaxHealth);
                currentCharacter.enemyHealth.OnTakeDamage += healthUI.UpdateHealthBar;
                currentCharacter.enemyHealth.OnDeath += healthUI.HandleDeath;

                // Subscribe the hit effects to the gun
                //currentCharacter.gunHandler.ActiveGun.OnSomethingHit += HitDamageable;
            }

            PlayerMaster.Instance.currentChar = currentCharacter.characterContainer;
           

        }

        private void HitDamageable(IDamageable hitDamageable)
        {
            if(!hitDamageable.IsWeakPoint)
            {
                // normal hit effects
                hitMarkerSRef.HitReaction(false);
            }
            else
            {
                // crit hit effects
                hitMarkerSRef.HitReaction(true);
            }
        }

        public void SwapCharacter(GameObject newChar, Transform position)//checkpoint stuff 
        {
            //Debug.Log("WE ARE GIVING VAR TO CHAR" + checkPointL);
            //Debug.Log("Location in PM " + PlayerMaster.Instance.checkPointLocation.position);
            //newChar.transform.position = position.position;
            SwapCharacter(newChar);
            //Debug.Log(newChar.transform.position);
            //Debug.Log("StateMachineInfo");
            if(checkPointL!=Vector3.zero)
            {
                currentCharacter.controller.enabled = false;
                newChar.transform.position = checkPointL;
                //Debug.Log("WE ARE GIVING VAR TO CHAR" + newChar.transform.position);
                currentCharacter.controller.enabled = true;
            }
        }
        bool isSwapping = false;
        // The movement of the camera is handled by Cinemachine, this is mostly for the particle effects and enabling/disabling the enemy ai.
        private IEnumerator SwapAnimation(Transform startTransform, Transform endTransform, CharacterInfoReference startCharacter, CharacterInfoReference endCharacter)
        {
            if(startCharacter.characterInfo.characterContainer.TryGetComponent(out KeyCardScript startKeycardEnemy))
            {
                startKeycardEnemy.SwapOut();
            }
            if(endCharacter.characterInfo.characterContainer.TryGetComponent(out KeyCardScript endKeycardEnemy))
            {
                endKeycardEnemy.SwapIn();
            }
            startCharacter.characterInfo.enemyBrain.StopAllCoroutines();
            endCharacter.characterInfo.enemyBrain.StopAllCoroutines();
            
            //deactivate input
            isSwapping = true;
            InputManager.Instance.PlayerInput.enabled = false;
            gunSelector.DespawnActiveGun();
            // Instantiate the particle system at the camera's position and as a child of the camera
            Quaternion camRotation = cameraTransform.rotation;
            // create an offset 
            camRotation.y += 180f;

            ParticleSystem tunnel = Instantiate(particleMaster.MindSwapTunnel, cameraTransform.position, camRotation);
            // Play the particle system
            
            // Play the shocking particle effect on the heads of both the player and enemy.
            // Disable both the start and end object ai
            endCharacter.vCamParent.SetActive(true);
            startCharacter.vCamParent.SetActive(false);
            startCharacter.characterInfo.enemyBrain.enabled = false;
            endCharacter.characterInfo.enemyBrain.enabled = false;
            startCharacter.characterInfo.agent.enabled = false;
            endCharacter.characterInfo.agent.enabled = false;

            healthUI.Enabled(false);
            startCharacter.characterInfo.enemyHealth.OnTakeDamage -= healthUI.UpdateHealthBar;
            startCharacter.characterInfo.enemyHealth.OnDeath -= healthUI.HandleDeath;
            playerAudio.PlayOneShot(mindswapDuring);
            playerAudio.loop = true;


            // Wait for the player to be far enough away from the startTransform to re enable its body
            while (Vector3.Distance(startTransform.position, cameraTransform.position) < 1f)//DURING MINDSWAP
            {
                tunnel.transform.position = cameraTransform.position;
                
                /* if(playingDuring == false)
                {
                    playerAudio.PlayOneShot(mindswapDuring);
                    playerAudio.loop = true;
                    playingDuring = true;
        
                } */
                
                
                yield return null;
            }
            startCharacter.ActivateThirdPersonModel();
            // now wait till the player is close enough to the endTransform to disable its body
            while(Vector3.Distance(endTransform.position, cameraTransform.position) > 0.5f)
            {
                tunnel.transform.position = cameraTransform.position;
                yield return null;
            }

            startCharacter.DeactivatePlayerAllAtOnce();
            endCharacter.ActivatePlayerAllAtOnce();

            startCharacter.characterInfo.animMaster.PrepareAnimator();
            
            OnSwapPlayer?.Invoke(endCharacter.characterInfo.characterContainer.transform);
            // Destroy the particle system at the end of the swap animation

            tunnel.Stop();
            Destroy(tunnel.gameObject);

            //activate input
            InputManager.Instance.PlayerInput.enabled = true;
            /* if(playingEnd == false)
                {
                    playerAudio.PlayOneShot(mindswapEnd);
                    playerAudio.loop = false;
                    playingEnd = true;
                    playingDuring = false;
                    playingBeginning = false;
                } */
            playerAudio.Stop();
            playerAudio.PlayOneShot(mindswapEnd);//end of swap audio
            armsOff.SetActive(true);
            playerAudio.loop = false;

            // rotate the character containers for both the previous and current character to match the model rotation
            // This should fix a bug where the input doesn't match what the player is doing.
            startCharacter.characterInfo.characterContainer.transform.rotation = startCharacter.characterInfo.model.transform.rotation;
            endCharacter.characterInfo.characterContainer.transform.rotation = endCharacter.characterInfo.model.transform.rotation;
            startCharacter.characterInfo.enemyBrain.enabled = true;

            healthUI.SetHealthBarOnSwap(endCharacter.characterInfo.enemyHealth.CurrentHealth, endCharacter.characterInfo.enemyHealth.MaxHealth);
            endCharacter.characterInfo.enemyHealth.OnTakeDamage += healthUI.UpdateHealthBar;
            endCharacter.characterInfo.enemyHealth.OnDeath += healthUI.HandleDeath;
            healthUI.Enabled(true);

            gunSelector.SetupGun(endCharacter.characterInfo.gunHandler.StartGun);
            isSwapping = false;
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
            if(isSwapping) return;
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
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out var firstHit, 1f, vaultLayers, QueryTriggerInteraction.Ignore))
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
                if (Physics.Raycast(currentCharacter.wallCheck.position, relativeDir, 2.5f, wallLayer, QueryTriggerInteraction.Ignore))
                    return true;
            }
            return false;
        }
        #endregion

        #region Ground Check

        Vector3 castDirection = Vector3.down;
        float castDistance = 0.0f;
        Vector3 boxSize = new Vector3(1f, 0.1f, 1f);//GROUND KEVIN CHANGE .4F .1F,.4F Old nums
        private bool GroundedCheck()
        {
            RaycastHit[] hits = Physics.BoxCastAll(currentCharacter.characterContainer.transform.position, boxSize, castDirection, Quaternion.identity, castDistance, groundLayer, QueryTriggerInteraction.Ignore);
            if(hits.Any(hit => hit.collider != null))
                return true;
            
            return false;
        }

        private bool AboveGround()
        {
            RaycastHit[] hits = Physics.BoxCastAll(currentCharacter.characterContainer.transform.position, boxSize, castDirection, Quaternion.identity, 0.2f, groundLayer, QueryTriggerInteraction.Ignore);
            if(hits.Any(hit => hit.collider != null))
                return false;
            
            return true;
        }
        #endregion

        public void SwitchMode(bool trySetStatic)
        {
            //if(StaticBar.instance.currentStatic >= 1)
            //{
                StaticMode = trySetStatic;
            //}


            //trySetStatic = false;
        }
        void OnDrawGizmos()
        {                
            if(Application.isPlaying)
            {
                bool isHit;
                if(currentCharacter != null && stateMachine != null)
                {
                    Gizmos.color = stateMachine.GetGizmoColor();
                    Gizmos.DrawCube(currentCharacter.characterContainer.transform.position + Vector3.up * 3f, Vector3.one);
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
                Gizmos.color = Color.yellow;
                Vector3 startPos = currentCharacter.characterContainer.transform.position + Vector3.up * 4f;
                Vector3 normal = startPos + (WallStateVariables.Instance.LastWallNormal * 4f);
                Gizmos.DrawLine(startPos, normal);
                Gizmos.DrawSphere(normal, 0.1f);
           }
        }
        public void PleaseSetLocationGODPLEASE(Transform location, bool isQuit)
        {
            if(isQuit == false)
                checkPointL = location.position;
            else
                checkPointL = Vector3.zero;
        }

        void OnValidate()
        {
            wallJumpAngle = wallJumpAngle > 1 ? 1 : wallJumpAngle;
            wallJumpAngle = wallJumpAngle < 0 ? 0 : wallJumpAngle;
        }
    } 
}