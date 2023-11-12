using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

using CharacterInfo = Player.CharacterInfo;


public class BasicEnemy : EnemyBrain
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
        if(CharacterInfo.agent.enabled && isActive)
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
            stateMachine.SetState(guardState, true);
        else
            stateMachine.SetState(patrolState, true);
    }

    protected override void HandleReactivation()
    {
        var state = stateMachine._currentState;
        
        stateMachine.SetState(stateMachine.defaultState);
        stateMachine.SetState(state);
    }
}
