using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SmartEnemy : EnemyBrain
{
    [SerializeField] private List<Vector3> patrolPoints;
    [HideInInspector] public int CurrentPatrolPointIndex { get; set; } = 0;
    
    [SerializeField] bool DebugMode = false;
    protected override void SetUp()
    {
        VariableSetup();
        StateMachineSetup();
    }

    void Update()
    {
        if(CharacterInfo.agent.enabled && isActive)
            stateMachine.Tick();
    }

    public override void StateMachineSetup()
    {
        stateMachine = new StateMachine.StateMachine();


        var runToCoverState = new RunToCoverState(this, agression);
        var delayState = new DelayState(2f);
        var coverState = new AtCoverState(this, agression);
        var debugState = new DebugState();

        AT(runToCoverState, delayState, HasReachedDestination());
        AT(delayState, coverState, delayState.IsDone());
        AT(debugState, runToCoverState, () => !DebugMode);
        ANY(debugState, () => DebugMode);
        

        stateMachine.SetState(runToCoverState);
    }

    void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        Gizmos.color = stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 6, 0.4f);     
    }
}