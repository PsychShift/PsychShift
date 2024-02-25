using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBrain : EnemyBrain
{
    ChaseState chaseState;//Started Happening because of AT statement in enemy brain from standing to chase
    protected override void SetUp()
    {
        Agent.enabled = true;
        StartCoroutine(WaitPlease());
    }
    public override void StateMachineSetup()
    {
        chaseState = new ChaseState(this);
        stateMachine.SetState(chaseState);
        //chaseState.OnEnter();
    }
    /* void OnDestroy()
    {
        chaseState.OnExit();
    } */

    IEnumerator WaitPlease()
    {
        yield return new WaitForSeconds(0.05f);
        VariableSetup();
        StateMachineSetup();
    }
    // Update is called once per frame
    void Update()
    {
        if(IsActive && chaseState != null)
            chaseState.Tick();
    }
}
