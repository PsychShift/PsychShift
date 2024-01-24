using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

using CharacterInfo = Player.CharacterInfo;


public class BasicEnemy : EnemyBrain
{
    [SerializeField] private bool isGuard;
    [SerializeField] private List<Vector3> patrolPoints;
    [HideInInspector] public int CurrentPatrolPointIndex { get; set; } = 0;
    
    protected override void SetUp()
    {
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
        stateMachine = new StateMachine.StateMachine();

        var patrolState = new PatrolState(this, agression, patrolPoints);
        var guardState = new GuardState(this, agression, transform.position);
        var chaseState = new ChaseState(this, agression);

        
        AT(patrolState, chaseState, PlayerInSight());
        AT(chaseState, patrolState, OutOfRangeForTooLong(agression.StopChasingTime));

        AT(guardState, chaseState, PlayerInSight());
        AT(chaseState, guardState, OutOfRangeForTooLongAndIsGuard(agression.StopChasingTime));

        stateMachine.AddAnyTransition(chaseState, WasDamaged());

        //if(isGaurd)
        stateMachine.SetState(guardState, true);
        //else
        //    stateMachine.SetState(patrolState, true);
    }

    private void OnDrawGizmos() {
        if(stateMachine == null) return;
        Gizmos.color = stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 6, 0.4f);
    }
}
