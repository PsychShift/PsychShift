using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBrain : EnemyBrain
{
    public Vector3[] patrolPath = new Vector3[0];
    PatrolState patrolState;
    LookAroundState lookAroundState;
    ChaseState chaseState;
    protected override void SetUp()
    {
        Agent.enabled = true;
        SetUpWait = StartCoroutine(WaitPlease());
    }

    public override void StateMachineSetup()
    {
        if(patrolPath.Length == 0)
        {
            patrolPath = new Vector3[1]
            {
                Vector3.zero
            };
        }
        patrolState = new PatrolState(this, patrolPath);

        lookAroundState = new LookAroundState(this);
        chaseState = new ChaseState(this);

        AT(patrolState, lookAroundState, patrolState.IsDone());
        AT(patrolState, chaseState, PlayerInSight());

        AT(lookAroundState, patrolState, lookAroundState.IsDone());
        AT(lookAroundState, chaseState, PlayerInSight());

        ANY(chaseState, WasDamaged());

        stateMachine.SetState(patrolState);
    }

    void Update()
    {
        if(_isActive && stateMachine != null)
            stateMachine.Tick();
    }
    public void SpawnerSetup(Vector3[] guardPos)
    {
        if(guardPos.Length == 0) return;
        StopCoroutine(SetUpWait);
        VariableSetup();
        StateMachineSetup();

        patrolPath = guardPos;
        patrolState = new PatrolState(this, guardPos);

        AT(patrolState, lookAroundState, patrolState.IsDone());
        AT(patrolState, chaseState, PlayerInSight());

        stateMachine.SetState(patrolState);
        _isActive = true;
    }
    int len;
    void OnValidate()
    {
        len = patrolPath.Length;
    }
    void OnDrawGizmos()
    {
        if(len > 0)
        {
            for(int i = 0; i < len; i++)
            {
                Gizmos.DrawWireSphere(patrolPath[i], .3f);
            }
        }
    }
}