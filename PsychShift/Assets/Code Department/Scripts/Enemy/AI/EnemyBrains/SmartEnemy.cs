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
        Agent.enabled = true;
        VariableSetup();
        StateMachineSetup();
    }

    void Update()
    {
        if(CharacterInfo.agent.enabled && IsActive)
            stateMachine.Tick();
    }

    public override void StateMachineSetup()
    {
        var runToCoverState = new RunToCoverState(this);
        var delayState = new DelayState(2f);
        var coverState = new AtCoverState(this);
        var debugState = new DebugState();

        AT(runToCoverState, delayState, HasReachedDestination());
        AT(delayState, coverState, delayState.IsDone());
        AT(debugState, runToCoverState, () => !DebugMode);
        ANY(debugState, () => DebugMode);
        stateMachine.AddAnyTransition(coverState, WasDamaged());
        
        stateMachine.SetState(runToCoverState, true);
    }

    void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        Gizmos.color = stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 6, 0.4f);     
    }
}