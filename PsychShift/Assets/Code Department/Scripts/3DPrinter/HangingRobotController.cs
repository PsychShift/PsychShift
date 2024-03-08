using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
    [SerializeField] private NavMeshAgent agent;
    public Transform target;
    public Transform model;
    private StateMachine.StateMachine attacksStateMachine;

    [SerializeField] private EnemyLauncher enemyLauncher;
    [SerializeField] private LaserShooter simpleLaser1;
    [SerializeField] private LaserBeamStats simpleLaserStats1;
    [SerializeField] private LaserBeamStats tailLaserStats1;

    public Vector3 Velocity { get { return agent.velocity; } }

    #region State References
    DecisionState decisionState;
    SpawnEnemyState spawnEnemyState;
    StaticLaserShotState staticLaserShotState;
    SweepingLaserState sweepingLaserState;
    RotatingLaserState rotatingTailLaserState;
    #endregion

    void Awake()
    {
        //Eventually this will be a sub state. Each phase of the boss will be its own state machine, and a healthgate will be the transition for the state machine states.
        attacksStateMachine = new StateMachine.StateMachine();
        decisionState = new DecisionState();
        spawnEnemyState = new SpawnEnemyState(this, enemyLauncher, animController.armsController);
        staticLaserShotState = new StaticLaserShotState(this, animController, animController.armsController, simpleLaser1, simpleLaserStats1);
        sweepingLaserState = new SweepingLaserState(this, animController, animController.armsController, simpleLaser1, simpleLaserStats1);
        rotatingTailLaserState = new RotatingLaserState(this, animController, stingerController, simpleLaser1, tailLaserStats1);

        void AT(IState from, IState to, Func<bool> condition) => attacksStateMachine.AddTransition(from, to, condition);

        AT(staticLaserShotState, rotatingTailLaserState, staticLaserShotState.IsFinished);
        AT(rotatingTailLaserState, decisionState, rotatingTailLaserState.IsDone);
        AT(decisionState, staticLaserShotState, decisionState.IsDone);
        attacksStateMachine.SetState(staticLaserShotState);    
    }

    void Update()
    {
        attacksStateMachine.Tick();
        //animController.RotateTowardsTarget(target.position);
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
}
