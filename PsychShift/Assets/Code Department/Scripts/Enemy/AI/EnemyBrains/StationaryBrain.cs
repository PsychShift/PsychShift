using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBrain : EnemyBrain
{
    [Tooltip("If this enemy is placed manually, you don't need to set this value. \n If it comes from a spawner, the spawner will set the value.")]
    public Vector3 guardPosition;
    protected override void SetUp()
    {
        Agent.enabled = true;
        VariableSetup();
        StateMachineSetup();
    }
    public override void StateMachineSetup()
    {
        stateMachine = new StateMachine.StateMachine();

        if(!SpawnerEnemy)
            guardPosition = transform.position;

        var returnToStationState = new SetLocationState(this, guardPosition);
        var chaseState = new ChaseState(this);
        var lookAroundState = new LookAroundState(this);


        AT(lookAroundState, chaseState, PlayerInSight());
        AT(chaseState, returnToStationState, CanGuard());
        AT(returnToStationState, lookAroundState, returnToStationState.IsDone());
        AT(returnToStationState, chaseState, PlayerInSight());


        if (SpawnerEnemy)
        {
            stateMachine.SetState(returnToStationState);
        }
        else
        {
            stateMachine.SetState(lookAroundState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive)
            stateMachine.Tick();
    }

    void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        Gizmos.color = stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 6, 0.4f);
    }
}