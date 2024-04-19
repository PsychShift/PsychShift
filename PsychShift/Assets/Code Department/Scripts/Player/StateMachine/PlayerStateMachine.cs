using System;
using UnityEngine;
using Cinemachine;
using System.Linq;
using System.Collections;
using Guns.Demo;
using UnityEngine.UI;
// reeeeeeeeeeeee
namespace Player
{
    [RequireComponent(typeof(InputManager))]
    public class PlayerStateMachine : MonoBehaviour
    {
        public Image debugSprite;
        private static PlayerStateMachine instance = null;
        public static PlayerStateMachine Instance 
        { 
            get 
            { 
                return instance; 
            }
        }
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
        #endregion
        #region Movement Variables
        [Header("Movement Variables")]
        private Vector3 currentMovement;
        private Vector3 appliedMovement;
        //private Vector2 currentInput;
        public float AppliedMovementX { get { return appliedMovement.x; } set { appliedMovement.x = value; }}
        //public float CurrentMovementX { get { return currentMovement.x; } set { currentMovement.x = value; }}
        public float AppliedMovementY { get { return appliedMovement.y; } set { appliedMovement.y = value; }}
        public float CurrentMovementY { get { return currentMovement.y; } set { currentMovement.y = value; }}
        public float AppliedMovementZ { get { return appliedMovement.z; } set { appliedMovement.z = value; }}


        /* public float CurrentMovementX { get { return currentInput.x; } set { currentInput.x = value; }}
        public float CurrentMovementZ { get { return currentInput.y; } set { currentInput.y = value; }} */

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
        /* [SerializeField] private Vector3 gravityDirection;
        public Vector3 GravityDirection { get { return gravityDirection; } set { gravityDirection = value; } } */
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
        [SerializeField] Animator handsAnimator;

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
        public GameObject tempCharacter;
        public CharacterInfo currentCharacter;
        private static Vector3 checkPoint;
        #region Monobehaviours
        public HitEffects hitMarkerSRef;

        void OnEnable()
        {
            instance = this;
            if(PlayerMaster.Instance == null)
            {
                GameObject playerMaster = new GameObject();
                playerMaster.name = "PlayerMaster";
                playerMaster.AddComponent<PlayerMaster>();
                Load();
            }
        }
        public void Load()
        {
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
            SwapCharacter(tempCharacter, tempCharacter.transform);
            // This is the first character, if it has a the FirstEnemy script, subrsribe its Swap function to the OnSwapPlayer event.
            if(currentCharacter != null)
            {
                if(currentCharacter.characterContainer.TryGetComponent(out FirstEnemy firstEnemy))
                {
                    firstEnemy.playerStateMachine = this;
                    OnSwapPlayer += firstEnemy.Swap;
                }
            }
            
            stateMachine = new StateMachine.StateMachine();

            #region Function Events
            InputManager.Instance.OnSlowActionStateChanged += SlowMotion;
            InputManager.Instance.OnSwapPressed += SwapPressed;
            InputManager.Instance.OnManipulatePressed += Manipulate;
            InputManager.Instance.OnSwitchPressed += SwitchMode;
            InputManager.Instance.OnInteractPressed += TryInteract;
            InputManager.Instance.OnMeleePressed+= TryMelee;
            #endregion

            // Create instances of root states
            var groundState = new GroundedState(this);
            var fallState = new FallState(this);
            var jumpState = new JumpState(this);
            var wallFlowState = new WallFlowState(this);
            var wallStaticState = new WallStaticState(this);
            var wallJumpState = new WallJumpState(this, wallJumpForce, wallJumpAngle);
            var wallFallState = new WallFallState(this, wallJumpForce, wallJumpAngle);

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

            #region Conditions
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
            #endregion

            stateMachine.SetState(groundState);
        }
        void OnDisable()
        {
            instance = null;
            currentCharacter.enemyHealth.OnTakeDamage -= HealthUI.Instance.UpdateHealthBar;
            currentCharacter.enemyHealth.OnDeath -= HealthUI.Instance.HandleDeath;
            OnSwapPlayer -= EnemyTargetManager.Instance.SetPlayer;
            InputManager.Instance.OnSlowActionStateChanged -= SlowMotion;
            InputManager.Instance.OnSwapPressed -= SwapPressed;
            InputManager.Instance.OnManipulatePressed -= Manipulate;
            InputManager.Instance.OnSwitchPressed -= SwitchMode;
            InputManager.Instance.OnInteractPressed-= TryInteract;
        }
        void Update()
        {
            
            if (isSlowed)
                SearchForInteractable();

            currentCharacter.animator.SetFloat("speed", 0);
        }
        void FixedUpdate()
        {
            RotatePlayer();
            stateMachine.Tick();

            //Vector2 debug = new Vector2(appliedMovement.x, appliedMovement.z);
            //Debug.Log(debug.magnitude + " " + debug);
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

        public void SwapCharacter(GameObject newCharacter)
        {
            
            if(newCharacter == null) return;
            CharacterInfoReference newCharacterInfoReference = newCharacter.GetComponent<CharacterInfoReference>();
            CharacterInfo newCharInfo = newCharacterInfoReference.characterInfo;
            if(newCharInfo.modifier == EEnemyModifier.NonSwap) return;
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
                    //currentCharacter.gunHandler.ActiveGun.OnSomethingHit -= HitDamageable;
                    //newCharInfo.gunHandler.ActiveGun.OnSomethingHit += HitDamageable;

                    currentCharacter = newCharInfo;
                    //currentCharacter.gunHandler.ActiveGun.OnSomethingHit += HitDamageable;
                    
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
                HealthUI.Instance.SetHealthBarOnSwap(currentCharacter.enemyHealth.CurrentHealth, currentCharacter.enemyHealth.MaxHealth);

                walkSpeed = newCharacterInfoReference.characterInfo.gunHandler.ActiveGun.CharacterConfig.WalkMoveSpeed;
                wallSpeed = newCharacterInfoReference.characterInfo.gunHandler.ActiveGun.CharacterConfig.WallMoveSpeed;

                currentCharacter.enemyHealth.OnTakeDamage += HealthUI.Instance.UpdateHealthBar;
                currentCharacter.enemyHealth.OnDeath += HealthUI.Instance.HandleDeath;

                currentCharacter.enemyHealth.SetMaxHealth(currentCharacter.gunHandler.ActiveBaseGun);
                // Subscribe the hit effects to the gun
                //currentCharacter.gunHandler.ActiveGun.OnSomethingHit += HitDamageable;
                
                
                
                
            }

            PlayerMaster.Instance.currentChar = currentCharacter.characterContainer;
        }

        /* private void HitDamageable(IDamageable hitDamageable)
        {
            Debug.Log("HIT?");
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
        } */

        public void SwapCharacter(GameObject newChar, Transform position)
        {
            SwapCharacter(newChar);

            if(checkPoint!=Vector3.zero)
            {
                currentCharacter.controller.enabled = false;
                newChar.transform.position = checkPoint;
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

            HealthUI.Instance.Enabled(false);
            startCharacter.characterInfo.enemyHealth.OnTakeDamage -= HealthUI.Instance.UpdateHealthBar;
            startCharacter.characterInfo.enemyHealth.OnDeath -= HealthUI.Instance.HandleDeath;
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

            HealthUI.Instance.SetHealthBarOnSwap(endCharacter.characterInfo.enemyHealth.CurrentHealth, endCharacter.characterInfo.enemyHealth.MaxHealth);
            endCharacter.characterInfo.enemyHealth.OnTakeDamage += HealthUI.Instance.UpdateHealthBar;
            endCharacter.characterInfo.enemyHealth.OnDeath += HealthUI.Instance.HandleDeath;
            HealthUI.Instance.Enabled(true);

            gunSelector.SetupGun(endCharacter.characterInfo.gunHandler.StartGun);
            startCharacter.characterInfo.enemyBrain.onSwappedOut?.Invoke(startTransform);
            endCharacter.characterInfo.enemyBrain.onSwappedIn?.Invoke(startTransform);

            walkSpeed = endCharacter.characterInfo.gunHandler.ActiveGun.CharacterConfig.WalkMoveSpeed;
            wallSpeed = endCharacter.characterInfo.gunHandler.ActiveGun.CharacterConfig.WallMoveSpeed;
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
            currentCharacter.characterContainer.transform.rotation = Quaternion.Euler(new Vector3(0, cameraTransform.localRotation.eulerAngles.y, 0));
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
            StaticMode = trySetStatic;
        }
        public void TryInteract()
        {
            
            RaycastHit[] hitInteract = Physics.BoxCastAll(cameraTransform.position, boxHalfExtents, cameraTransform.forward,Quaternion.identity, 0, ~0, QueryTriggerInteraction.Collide );
            if(hitInteract.Length>0)
            {
                Debug.Log("Hit a box");
                for(int i = 0; i<hitInteract.Length;i++)
                {
                    Debug.Log(hitInteract[i].collider.gameObject.name);
                    if(hitInteract[i].collider.TryGetComponent(out DataLog dataLog))
                    {
                        dataLog.TextInteract();
                        //break;
                        return;
                    }
                }
                    
                        
            }
        }

        public void TryMelee()
        {
            handsAnimator.SetTrigger("MeleeTrigger");
        }
        void OnDrawGizmos()
        {                
            if(Application.isPlaying)
            {
                bool isHit;
                if(currentCharacter != null && stateMachine != null)
                {
                    Gizmos.color = stateMachine.GetGizmoColor();
                    if(debugSprite != null)
                        debugSprite.color = Gizmos.color;
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
        public void SetLocation(Transform location)
        {
            if(location != null)
                checkPoint = location.position;
            else
                checkPoint = Vector3.zero;
        }

        void OnValidate()
        {
            wallJumpAngle = wallJumpAngle > 1 ? 1 : wallJumpAngle;
            wallJumpAngle = wallJumpAngle < 0 ? 0 : wallJumpAngle;
        }
    } 
}