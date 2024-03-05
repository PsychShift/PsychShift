using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightBrain : EnemyBrain
{
    private RigColliderManager rigColliderManager;
    ChaseState chaseState;//Started Happening because of AT statement in enemy brain from standing to chase

    public void Reset()
    {
        stateMachine.SetState(ragdollState);
        EnemyHealth.ResetHealth();
    }
    protected override void SetUp()
    {
        Agent.enabled = true;
        rigColliderManager = GetComponent<RigColliderManager>();
        StartCoroutine(WaitPlease());
    }
    public override void StateMachineSetup()
    {
        chaseState = new ChaseState(this);
        stateMachine.SetState(ragdollState);
        AT(standupState, chaseState, BackToChase());
    }

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
            stateMachine.Tick();
    }
    public void Launch(Vector3 dir)
    {
        stateMachine.SetState(ragdollState);
        int len = rigColliderManager.Rigidbodies.Length;

        for(int i = 0; i < len; i++)
        {
            rigColliderManager.Rigidbodies[i].AddForce(dir * 100, ForceMode.Impulse);
        }
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
