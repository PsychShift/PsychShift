using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HangingRobotController : MonoBehaviour
{
    [Header("Everything Else")]

    // Lets start with movement, we need to keep track of the player's
    [SerializeField] private HangingAnimatorController animController;
    [SerializeField] private StingerController stingerController;
    [HideInInspector] public NavMeshAgent agent;
    public Transform target;
    public NavMeshQueryFilter filter;
    public Transform model;
    private StateMachine.StateMachine stateMachine;

    [SerializeField] private LaserShooter simpleLaser1;
    [SerializeField] private LaserBeamStats simpleLaserStats1;
    [SerializeField] private LaserBeamStats tailLaserStats1;

    public Vector3 Velocity { get { return agent.velocity; } }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        filter.areaMask = NavMesh.GetAreaFromName("Hanging"); // this is the area mask for the Hanging NavMesh Surface

        stateMachine = new StateMachine.StateMachine();

        /*
        States: 
        Idle,
        Do Attack 1,
        Do Attack 2,
        Do Attack 3,
        Do weird thing?,

        Eventually this will be a sub state. Each phase of the boss will be its own state machine, and a healthgate will be the transition for the state machine states.
        */

        var decisionState = new DecisionState();
        var staticLaserShotState = new StaticLaserShotState(this, animController, animController.armsController, simpleLaser1, simpleLaserStats1);
        var sweepingLaserState = new SweepingLaserState(this, animController, animController.armsController, simpleLaser1, simpleLaserStats1);
        var rotatingTailLaserState = new RotatingLaserState(this, animController, stingerController, simpleLaser1, tailLaserStats1);

        void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);

        AT(staticLaserShotState, rotatingTailLaserState, staticLaserShotState.IsFinished);
        AT(rotatingTailLaserState, decisionState, rotatingTailLaserState.IsDone);
        AT(decisionState, staticLaserShotState, decisionState.IsDone);
        stateMachine.SetState(staticLaserShotState);
        
        //StartCoroutine(FollowPlayer());
    }

    void Update()
    {
        stateMachine.Tick();
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
}
