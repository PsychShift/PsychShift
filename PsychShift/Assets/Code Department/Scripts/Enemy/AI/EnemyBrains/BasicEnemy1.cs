using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

using CharacterInfo = Player.CharacterInfo;


public class BasicEnemy1 : EnemyBrain
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
        if(CharacterInfo1.agent.enabled)
            stateMachine.Tick();
    }

    public override void StateMachineSetup()
    {
        stateMachine = new StateMachine.StateMachine();

        var patrolState = new PatrolState1(this, agression, patrolPoints);
        var guardState = new GuardState1(this, agression, transform.position);
        var chaseState = new ChaseState1(this, agression);

        
        AT(patrolState, chaseState, PlayerInSight());
        AT(chaseState, patrolState, OutOfRangeForTooLong(agression.StopChasingTime));

        AT(guardState, chaseState, PlayerInSightWide());
        AT(chaseState, guardState, OutOfRangeForTooLongAndIsGuard(agression.StopChasingTime));

        if(isGaurd)
            stateMachine.SetState(guardState);
        else
            stateMachine.SetState(patrolState);
    }


}
