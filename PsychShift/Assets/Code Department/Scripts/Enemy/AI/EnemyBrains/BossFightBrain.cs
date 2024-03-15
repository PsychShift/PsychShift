using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFightBrain : EnemyBrain
{
    private RigColliderManager rigColliderManager;
    ChaseState chaseState;
    LaunchedRagdollState _ragdollState;
    

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
        stateMachine = new();
        _ragdollState = new LaunchedRagdollState(this, rigColliderManager, GetComponent<TempGravity>(), rigColliderManager.Rigidbodies);
        chaseState = new ChaseState(this);
        
        AT(ragdollState, chaseState, NotGrounded());
        AT(_ragdollState, standupState, () => _ragdollState.IsDone());
        AT(standupState, chaseState, BackToChase());
    }
    // Update is called once per frame
    void Update()
    {
        if(_isActive && chaseState != null)
            stateMachine.Tick();
    }
    public IEnumerator Launch(Vector3 dir, float force)
    {
        yield return new WaitForSeconds(0.06f);
        _ragdollState.force = dir*force;
        stateMachine.SetState(_ragdollState);
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
