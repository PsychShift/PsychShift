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
    bool isSpawnerBoy = false;
    protected override void SetUp()
    {
        Agent.enabled = true;
        SetUpWait = StartCoroutine(WaitPlease());
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

        _isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(_isActive)
            stateMachine.Tick();
    }

    public void SpawnerSetup(Vector3 guardPos)
    {
        StopCoroutine(SetUpWait);
        VariableSetup();
        StateMachineSetup();
        isSpawnerBoy = true;
        Debug.Log(guardPos + "spawner set up");
        if(guardPos == Vector3.zero) return;
        returnToStationState = new SetLocationState(this, guardPos);
        AT(returnToStationState, lookAroundState, returnToStationState.IsDone());

        stateMachine.SetState(returnToStationState);
        _isActive = true;
    }

    void OnDrawGizmos()
    {
        if(Application.isPlaying && stateMachine != null)
        {
            Gizmos.color = stateMachine.GetGizmoColor();
            Gizmos.DrawCube(transform.position, Vector3.one * 5);
        }
    }
}