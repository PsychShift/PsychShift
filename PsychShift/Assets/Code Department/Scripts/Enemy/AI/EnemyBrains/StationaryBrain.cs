using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBrain : EnemyBrain
{
    [Tooltip("If this enemy is placed manually, you don't need to set this value. \n If it comes from a spawner, the spawner will set the value.")]
    public Vector3 guardPosition;
    ChaseState chaseState;
    SetLocationState returnToStationState;
    LookAroundState lookAroundState;
    protected override void SetUp()
    {
        Agent.enabled = true;
        StartCoroutine(WaitPlease());
    }
    public override void StateMachineSetup()
    {
        guardPosition = transform.position;

        returnToStationState = new SetLocationState(this, guardPosition);
        chaseState = new ChaseState(this);
        lookAroundState = new LookAroundState(this);


        AT(lookAroundState, chaseState, PlayerInSight());
        //AT(chaseState, returnToStationState, CanGuard());
        AT(returnToStationState, lookAroundState, returnToStationState.IsDone());
        AT(returnToStationState, chaseState, PlayerInSight());

        AT(lookAroundState, chaseState, WasDamaged());
        AT(returnToStationState, chaseState, WasDamaged());

        stateMachine.SetState(lookAroundState,true);
        
        AT(standupState, chaseState, BackToChase());
    }

    // Update is called once per frame
    void Update()
    {
        if(IsActive)
            stateMachine.Tick();
    }

    public void SpawnerSetup(Vector3 guardPos)
    {
        if(guardPos == Vector3.zero) return;
        returnToStationState = new SetLocationState(this, guardPos);
        AT(returnToStationState, lookAroundState, returnToStationState.IsDone());

        stateMachine.SetState(returnToStationState);
    }
}