using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartEnemy : EnemyBrain
{
    [SerializeField] private List<Vector3> patrolPoints;
    [HideInInspector] public int CurrentPatrolPointIndex { get; set; } = 0;
    
    protected override void SetUp()
    {
        VariableSetup();
        StateMachineSetup();
    }

    void Update()
    {
        if(CharacterInfo.agent.enabled)
            stateMachine.Tick();
    }

    public override void StateMachineSetup()
    {
        stateMachine = new StateMachine.StateMachine();

        var patrolState = new PatrolState(this, agression, patrolPoints);
        var guardState = new GuardState(this, agression, transform.position);
        var chaseState = new ChaseState(this, agression);

        
        AT(patrolState, chaseState, PlayerInSight());
        AT(chaseState, patrolState, OutOfRangeForTooLong(agression.StopChasingTime));

        AT(guardState, chaseState, PlayerInSightWide());
        AT(chaseState, guardState, OutOfRangeForTooLongAndIsGuard(agression.StopChasingTime));

        if(isGaurd)
            stateMachine.SetState(guardState);
        else
            stateMachine.SetState(patrolState);
    }

    void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        Gizmos.color = stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 3, 0.4f);     
    }
}
