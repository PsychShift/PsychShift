using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBrain : EnemyBrain
{
    ChaseState chaseState;
    [HideInInspector] public Vector3 guardPosition;
    protected override void SetUp()
    {
        Agent.enabled = true;
        VariableSetup();
        StateMachineSetup();
    }

    public override void StateMachineSetup()
    {
        var pickRandom = new RandomLocationState(this);
        var moveToDestination = new MoveToDestinationState(this, () => pickRandom.Destination);
        
        var lookAround = new LookAroundState(this);
        chaseState = new ChaseState(this);

        AT(pickRandom, lookAround, pickRandom.IsDone());
        AT(lookAround, pickRandom, lookAround.IsDone());

        AT(pickRandom, chaseState, PlayerInSight());
        AT(lookAround, chaseState, PlayerInSight());

        ANY(chaseState, WasDamaged());

        stateMachine.SetState(pickRandom, true);
    }

    void Update()
    {
        if(IsActive && stateMachine != null)
            stateMachine.Tick();
    }

    void OnDrawGizmos()
    {
        if (stateMachine == null) return;
        Gizmos.color = stateMachine.GetGizmoColor();
        Gizmos.DrawSphere(transform.position + Vector3.up * 6, 0.4f);     
    }

    public void SpawnerSetup(Vector3 guardPos)
    {
        if(guardPos == Vector3.zero) return;
        var startHereState = new SetLocationState(this, guardPos);

        AT(startHereState, chaseState, startHereState.IsDone());
        AT(startHereState, chaseState, WasDamaged());

        stateMachine.SetState(startHereState);
    }
}
