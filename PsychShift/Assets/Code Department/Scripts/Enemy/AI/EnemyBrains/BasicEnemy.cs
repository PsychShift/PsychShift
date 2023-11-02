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
    
    void Awake()
    {
        if(!isActive) return;
        SetUp();
    }
    private void SetUp()
    {
        characterInfo = GetComponent<CharacterInfoReference>().characterInfo;
        VariableSetup();
        StateMachineSetup();
    }
    void Update()
    {
        if(!isActive) return;
        stateMachine.Tick();
    }

    public override void StateMachineSetup()
    {
        stateMachine = new StateMachine.StateMachine();

        var patrolState = new PatrolState(this, agression, patrolPoints);
        var chaseState = new ChaseState(this, agression);

        
        AT(patrolState, chaseState, PlayerInSight());
        AT(chaseState, patrolState, OutOfRangeForTooLong(agression.StopChasingTime));


        stateMachine.SetState(patrolState);
    }
    
}
