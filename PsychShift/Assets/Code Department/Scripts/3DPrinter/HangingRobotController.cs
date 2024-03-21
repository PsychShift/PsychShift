using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// on first shield, boss spawns rocket enemies, will continue spawning rocket enemies until you break a shield generator
// then he will omega beam all remaining rocket enemies and return to spawning normal enemies
// on the second shield spawn, a platform will come up to allow you to parkour to a side room, in this room you will activate a push pull object that destroys
// two shield generators, then you can return to the fight.
// on the final shield spawn, the boss will destroy the glass with a special attack, allowing you to swap to a rocket enemy to break the final shield
// then you can push the boss to the next phase
// i think phase two should be kinda short, just an intro to his jump attacks, also because I dont have that much time to do a lot.
// this phase will just be straight up combat, no puzzles
// phase three is the chase, same attacks as the second phase, more reliance on push/pull

public enum EBossStates
{
    SpawnWave,
    StaticArmLaser,
    SweepingLaser,
    TailLaser,
    None
}
public class HangingRobotController : MonoBehaviour
{
    [Header("Everything Else")]

    // Lets start with movement, we need to keep track of the player's
    [SerializeField] private HangingAnimatorController animController;
    [SerializeField] private StingerController stingerController;
    public NavMeshAgent agent {  get; private set; }
    public Transform target;
    public Transform model;
    public StateMachine.StateMachine attacksStateMachine;
    public IState[] attackStates;

    [SerializeField] private EnemyLauncher enemyLauncher;
    [SerializeField] private LaserShooter simpleLaser1;
    [SerializeField] private LaserBeamStats simpleLaserStats1;
    [SerializeField] private LaserBeamStats tailLaserStats1;

    [SerializeField] private List<Guns.GunType> defaultGunSpawns;
    [SerializeField] private List<EEnemyModifier> defaultModifiers;

    [HideInInspector] public List<Guns.GunType> guns;
    [HideInInspector] public List<EEnemyModifier> modifiers;


    #region Movement
    public StateMachine.StateMachine movementStateMachine;

    public bool canMove = false;
    public bool desiredHeightReached = false;

    [SerializeField] private float desiredY = 0;
    public float DesiredY 
    { 
        get { return desiredY; } 
        set 
        { 
            desiredY = value;
            desiredHeightReached = false;
            StartCoroutine(ChangeHeight(desiredY)); 
        } 
    }

    public Vector3 Velocity { get { return agent.velocity; } }

    private StationaryStateMB stationaryState;
    private MoveStateMB moveState;
    #endregion
    #region State References
    DecisionState decisionState;
    SpawnEnemyState spawnEnemyState;
    StaticLaserShotState staticLaserShotState;
    SweepingLaserState sweepingLaserState;
    RotatingLaserState rotatingTailLaserState;
    #endregion

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        void ATAttack(IState from, IState to, Func<bool> condition) => attacksStateMachine.AddTransition(from, to, condition);

        void ATMove(IState from, IState to, Func<bool> condition) => movementStateMachine.AddTransition(from, to, condition);
        void ANYMove(IState to, Func<bool> condition) => movementStateMachine.AddAnyTransition(to, condition);
        #region attacks
        //Eventually this will be a sub state. Each phase of the boss will be its own state machine, and a healthgate will be the transition for the state machine states.
        attacksStateMachine = new StateMachine.StateMachine();
        decisionState = new DecisionState(this);
        spawnEnemyState = new SpawnEnemyState(this, enemyLauncher, animController.armsController);
        staticLaserShotState = new StaticLaserShotState(this, animController, animController.armsController, simpleLaser1, simpleLaserStats1);
        sweepingLaserState = new SweepingLaserState(this, animController, animController.armsController, simpleLaser1, simpleLaserStats1);
        rotatingTailLaserState = new RotatingLaserState(this, animController, stingerController, simpleLaser1, tailLaserStats1);

        attackStates = new IState[]
        {
            spawnEnemyState,
            staticLaserShotState,
            rotatingTailLaserState
        };
            //sweepingLaserState,
        
        guns = defaultGunSpawns;
        modifiers = defaultModifiers;


        ATAttack(spawnEnemyState, decisionState, spawnEnemyState.IsDone);
        ATAttack(staticLaserShotState, decisionState, staticLaserShotState.IsFinished);
        ATAttack(rotatingTailLaserState, decisionState, rotatingTailLaserState.IsDone);
        //AT(decisionState, staticLaserShotState, decisionState.IsDone);
        attacksStateMachine.SetState(decisionState);
        #endregion

        #region Movement
        movementStateMachine = new StateMachine.StateMachine();

        stationaryState = new StationaryStateMB(this, animController);
        moveState = new MoveStateMB(this, animController);


       // ATMove(moveState, stationaryState, () => !canMove);
       // ATMove(stationaryState, moveState, () => canMove);

        ANYMove(stationaryState, () => !canMove);
        canMove = false;
        movementStateMachine.SetState(stationaryState);

        DesiredY = -100f;
        #endregion
    }

    void Update()
    {
        attacksStateMachine.Tick();
        if(canMove)
            Rotate();
    }

    private void FixedUpdate()
    {
        movementStateMachine.Tick();
        
    }

    void Rotate()
    {
        Vector3 direction = transform.position - target.position;
        direction.y = 0; // Ensure we're only considering the X and Z axes
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    public float speed = 1f;
    private IEnumerator ChangeHeight(float targetY)
    {
        Vector3 pos = model.position;
        while (Mathf.Abs(model.position.y - targetY) > 0.01f) // Check if we're close enough to stop
        {
            pos = model.position;
            pos.y = Mathf.MoveTowards(model.position.y, targetY, speed * Time.deltaTime);
            model.position = pos;
            yield return null;
        }
        pos = model.position;
        pos.y = targetY;
        model.position = pos;
        desiredHeightReached = true;
    }

    IEnumerator FollowPlayer()
    {
        if(target == null)
        {
            yield break;
        }

        while (true)
        {
            agent.SetDestination(target.position);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void SwitchState(EBossStates state)
    {
        switch(state)
        {
            case EBossStates.SpawnWave :
            attacksStateMachine.SetState(spawnEnemyState);
            break;

            case EBossStates.StaticArmLaser :
            attacksStateMachine.SetState(staticLaserShotState);
            break;

            case EBossStates.SweepingLaser :
            attacksStateMachine.SetState(sweepingLaserState);
            break;

            case EBossStates.TailLaser :
            attacksStateMachine.SetState(rotatingTailLaserState);
            break;

            case EBossStates.None :
            Debug.Log("Nah, ima do my own thing.");
            break;
        }
    }

    public IEnumerator WaitForHeight(StateMachine.StateMachine stateMachine, IState state, Action callback)
    {
        WaitForSeconds wait = new WaitForSeconds(3f);
        while (!desiredHeightReached)
        {
            yield return wait;
        }

        // Call the callback function after the desired height is reached
        callback?.Invoke();

        stateMachine.SetState(state);
    }
    public IEnumerator WaitForHeight(Action callback)
    {
        WaitForSeconds wait = new WaitForSeconds(3f);
        while (!desiredHeightReached)
        {
            yield return wait;
        }

        // Call the callback function after the desired height is reached
        callback?.Invoke();
    }

    public Vector3 pos;
    public float size;
    private void OnDrawGizmos()
    {
        if (attacksStateMachine == null) return;
        Gizmos.color = attacksStateMachine.GetGizmoColor();
        Gizmos.DrawCube(pos, Vector3.one * size);
    }
}
