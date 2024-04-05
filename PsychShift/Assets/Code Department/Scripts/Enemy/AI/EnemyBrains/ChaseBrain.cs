using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBrain : EnemyBrain
{
    ChaseState chaseState;//Started Happening because of AT statement in enemy brain from standing to chase
    protected override void SetUp()
    {
        Agent.enabled = true;
        SetUpWait = StartCoroutine(WaitPlease());
    }
    public override void StateMachineSetup()
    {
        chaseState = new ChaseState(this);
        stateMachine.SetState(chaseState);
        AT(standupState, chaseState, BackToChase());
    }
    /* void OnDestroy()
    {
        chaseState.OnExit();
    } */

    
    // Update is called once per frame
    void Update()
    {
        if(_isActive && chaseState != null)
            stateMachine.Tick();
    }

    public void SpawnerSetup(Vector3 guardPos)
    {
        if(guardPos == Vector3.zero) return;
        StopCoroutine(SetUpWait);
        VariableSetup();
        StateMachineSetup();

        var startHereState = new SetLocationState(this, guardPos);

        AT(startHereState, chaseState, startHereState.IsDone());
        AT(startHereState, chaseState, WasDamaged());

        stateMachine.SetState(startHereState);
        _isActive = true;
    }
}
