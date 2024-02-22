using UnityEngine;
using System;

using CharacterInfo = Player.CharacterInfo;
using UnityEngine.AI;
using System.Collections;
using Guns.Health;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Events;


#if UNITY_EDITOR
using UnityEditor;
#endif
[RequireComponent(typeof(CharacterInfoReference), typeof(TempGravity), typeof(FieldOfView))]
[DisallowMultipleComponent]
public abstract class EnemyBrain : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    public bool IsActive {
        get 
        { 
            return _isActive; 
        }
        set 
        { 
            if(value == false)
            {
                //Debug.Log("Deactivating");
                StopAllCoroutines();
            }
            else
            {
                //Debug.Log("Reactivating");
                HandleReactivation();
            }
            _isActive = value; 
        }
    }
    //[SerializeField] protected LayerMask playerLayer;
    protected StateMachine.StateMachine stateMachine;
    [SerializeField] private AIAgression _agression;
    public AIAgression agression
    {
        get
        {
            return _agression;
        }
        set
        {
            UpdateAgression(value);
            _agression = value;
        }
    }

    private CharacterInfo characterInfo = null;
    public CharacterInfo CharacterInfo {
        get {
            if(characterInfo == null)
            {
                characterInfo = gameObject.GetComponent<CharacterInfoReference>().characterInfo;
            }
            return characterInfo;
        }
    }

    public Animator Animator => CharacterInfo.animator;
    public NavMeshAgent Agent => CharacterInfo.agent;
    public Transform Model => CharacterInfo.model.transform;
    public EnemyAnimatorMaster AnimMaster => CharacterInfo.animMaster;
    public EnemyHealth EnemyHealth => CharacterInfo.enemyHealth;

    protected List<AbstractEnemyModifier> modifiers;
    
    [HideInInspector] public Cover currentCover;

    [HideInInspector] public Transform gauranteedPlayer => EnemyTargetManager.Instance.player;
    private Transform _player;
    private FieldOfView fovRef;
    public Transform player 
    { 
        get 
        { 
            if(_player == null)
                return gauranteedPlayer; 
            else
                return _player;
        } 
        set { _player = value; } 
    }
    private bool _spawnerEnemy;
    public bool SpawnerEnemy
    {
        get
        {
            return _spawnerEnemy;
        }

        set
        {
            if(value)
            {
                StateMachineSetup();
            }
            _spawnerEnemy = value;
        }
    }

    public delegate void OnSwappedDelegate(Transform t);
    public OnSwappedDelegate onSwappedIn;
    public OnSwappedDelegate onSwappedOut;

    protected RagdollState ragdollState;
    protected StandupState standupState;

    /// <summary>
    /// Any variables that require initialization before a Func<bool> is used should be initialized here.
    /// Call this function in the Awake() method of the inheriting class.
    /// </summary> 
    protected void VariableSetup()
    {
        RigColliderManager rgm = GetComponent<RigColliderManager>();
        ragdollState = new RagdollState(rgm);
        standupState = new StandupState(rgm);

        CharacterInfo.agent.speed = CharacterInfo.movementStats.moveSpeed;
        if(!TryGetComponent(out fovRef))
        {
            fovRef = gameObject.AddComponent<FieldOfView>();
        }
        foreach(var mod in GetComponents<AbstractEnemyModifier>())
        {
            mod.ApplyModifier(this);
        }
        UpdateAgression(agression);
        characterInfo = gameObject.GetComponent<CharacterInfoReference>().SetUp();
        EnemyHealth.OnDeath += Died;
    }
    private void Died(Transform idk)
    {
        EnemyHealth.OnDeath -= Died;
        IsActive = false;
        Agent.isStopped = false;
        //enabled = false;
    }

    protected void HandleReactivation()
    {
        //if(stateMachine._currentState is ICoroutineRestarter)
        //{
           // (stateMachine._currentState as ICoroutineRestarter).RestartCoroutine();
           // Debug.Log("Restarted Coroutine");
        //}
    }

    protected void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    protected void ANY(IState from, Func<bool> condition) => stateMachine.AddAnyTransition(from, condition);

    public abstract void StateMachineSetup();

    protected Func<bool> PlayerInSight() => () => fovRef.canSeePlayer;
    //protected Func<bool> PlayerInSightWide() => () => IsPlayerInSightWideView();
    protected Func<bool> PlayerInAttackRange() => () => IsPlayerInRange();
    protected Func<bool> OutOfRangeForTooLong(float maxTimeOutOfSight) => () => IsPlayerOutOfRangeForTooLong(maxTimeOutOfSight);
    protected Func<bool> OutOfRangeForTooLongAndIsGuard(float maxTimeOutOfSight) => () => IsPlayerOutOfRangeForTooLong(maxTimeOutOfSight);
    protected Func<bool> CanGuard() => () => !fovRef.canSeePlayer;
    protected Func<bool> FoundCover() => () => FindCover() != null;
    protected Func<bool> HasReachedDestination() => () => CharacterInfo.agent.remainingDistance <= 0.1f;
    protected Func<bool> WasDamaged() => () => wasHit;

    float time = 0f;
    public Cover FindCover()
    {
        currentCover = null;

        currentCover = CoverArea.Instance.GetCover(player);
        return currentCover;
    }
    private bool IsPlayerOutOfRangeForTooLong(float maxTimeOutOfSight)
    {
        if(!fovRef.canSeePlayer)
        {
            time += Time.deltaTime;
            if(time >= maxTimeOutOfSight)
                return true;

            return false;
        }
        else
        {
            time = 0f;
            return false;
        }
    }

    private bool wasHit = false;
    private void TookDamage(int dmg)
    {
        wasHit = true;
        StartCoroutine(NotAngryAfterHit());
    }
    IEnumerator NotAngryAfterHit()
    {
        yield return new WaitForSeconds(5f);
        wasHit = false;
    }
    protected abstract void SetUp();

    void OnEnable()
    {
        SetUp();
        CharacterInfo.enemyHealth.OnTakeDamage += TookDamage;
        /*CharacterInfo.agent.enabled = true;
        if(!CharacterInfo.controller.isGrounded)
        {
            CharacterInfo.agent.enabled = false;
            GetComponent<TempGravity>().enabled = true;
            StartCoroutine(WaitTillGrounded());
        } */
    }

    void OnDisable()
    {
        CharacterInfo.agent.enabled = false;
        CharacterInfo.enemyHealth.OnTakeDamage -= TookDamage;
    }

    private bool IsPlayerInRange()
    {
        float distance = Vector3.SqrMagnitude(transform.position - CharacterInfo.characterContainer.transform.position);
        return distance <= agression.DetectionRange;
    }

    public void CreateDebugSphere()
    {
        // create a sphere at the transform position
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = transform.position;
    }

    private void UpdateAgression(AIAgression agression)
    {
        if(agression == null || fovRef == null) return;

        fovRef.radius = agression.DetectionRange;
    }


    public void SetUpBrainSwap(CharacterBrainSwappingInfo info, EEnemyModifier[] modifiers)
    {
        agression = info.AIAgression;
        this.modifiers = new();
        Component[] allComponents = GetComponents<Component>();

        // Iterate backwards to avoid issues with modifying the array during iteration
        for (int i = allComponents.Length -  1; i >=  0; i--)
        {
            // Check if the component implements IModifier
            if (allComponents[i] is AbstractEnemyModifier)
            {
                // Destroy the component
                #if UNITY_EDITOR
                DestroyImmediate(allComponents[i]);
                #else
                Destroy(allComponents[i]);
                #endif
            }
        }
        foreach(var mod in modifiers)
        {
            
            switch(mod)
            {
                case EEnemyModifier.None : 
                    break;
                case EEnemyModifier.Explosive : 
                    AbstractEnemyModifier expMod = gameObject.AddComponent<ExplosiveDeathModifier>();
                    this.modifiers.Add(expMod);
                    break;
                case EEnemyModifier.Keycard :
                    AbstractEnemyModifier keyMod = gameObject.AddComponent<KeyCardModifier>();
                    this.modifiers.Add(keyMod);
                    break;
                case EEnemyModifier.NonSwap :
                    AbstractEnemyModifier nonswapMod = gameObject.AddComponent<NonSwapModifier>();
                    this.modifiers.Add(nonswapMod);
                    break;
                default :
                    break;
            }
        }
        if(Application.isPlaying)
        {
            VariableSetup();
        }
        #if UNITY_EDITOR
        if(!Application.isPlaying)
            EditorUtility.SetDirty(this);
        #endif
    }
}
/*     private IEnumerator WaitTillGrounded()
    {
        while(!CharacterInfo.controller.isGrounded)
        {
            yield return null;
        }
        GetComponent<TempGravity>().enabled = false;
        CharacterInfo.agent.enabled = true;
    } */
    /* void OnDrawGizmos()
    {
        if(!Application.isPlaying || !isActive) return;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(characterInfo.cameraRoot.position, characterInfo.cameraRoot.position + (characterInfo.cameraRoot.forward * agression.SphereCastDetectionRadius));
        Gizmos.color = Color.blue;

    } */
/*private bool IsPlayerInSight()
    {
        Physics.SphereCast(CharacterInfo.cameraRoot.position, agression.SphereCastDetectionRadius, 
        CharacterInfo.cameraRoot.forward, out RaycastHit hit, agression.DetectionRange, layerMask: playerLayer);
        if(hit.collider == null) return false;
        if(hit.collider.tag == "Player")
        {
            player = hit.collider.transform;
            return true;
        }
        return false;
    }
    private bool IsPlayerInSightWideView()
    {
        // Cast a sphere slightly to the left of transform.forward
        Vector3 leftDirection = Quaternion.Euler(0, -20, 0) * transform.forward;


        // Cast a sphere slightly to the right of transform.forward
        Vector3 rightDirection = Quaternion.Euler(0, 20, 0) * transform.forward;

        Physics.SphereCast(CharacterInfo.cameraRoot.position, agression.SphereCastDetectionRadius,
        CharacterInfo.cameraRoot.forward, out RaycastHit hit, agression.DetectionRange, layerMask: playerLayer);
        if (hit.collider == null) return false;
        if (hit.collider.tag == "Player")
        {
            player = hit.collider.transform;
            return true;
        }

        Physics.SphereCast(CharacterInfo.cameraRoot.position, agression.SphereCastDetectionRadius,
        leftDirection, out hit, agression.DetectionRange, layerMask: playerLayer);
        if (hit.collider == null) return false;
        if (hit.collider.tag == "Player")
        {
            player = hit.collider.transform;
            return true;
        }

        Physics.SphereCast(CharacterInfo.cameraRoot.position, agression.SphereCastDetectionRadius,
        rightDirection, out hit, agression.DetectionRange, layerMask: playerLayer);
        if (hit.collider == null) return false;
        if (hit.collider.tag == "Player")
        {
            player = hit.collider.transform;
            return true;
        }

        return false;
    }*/