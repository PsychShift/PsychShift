using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSuperState : IState
{
    protected StateMachine.StateMachine stateMachine;
    protected EnemyBrain brain;
    protected void SetUp(EnemyBrain brain)
    {
        this.brain = brain;
        stateMachine = new StateMachine.StateMachine();

        var idleShootState = new IdleShootState(brain);
        var activeShootState = new ActiveShootState(brain);
        var reloadState = new ReloadState(brain);

        // Transitions
        AT(idleShootState, activeShootState, idleShootState.IsDone());
        AT(activeShootState, idleShootState, activeShootState.IsDone());
        //AT(activeShootState, reloadState,() => brain.CharacterInfo.gunHandler.ShouldReload());
       //AT(reloadState, activeShootState, () => !brain.CharacterInfo.gunHandler.ShouldReload());

        stateMachine.SetState(activeShootState);
    }
    protected void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    protected void Any(IState from, Func<bool> condition) => stateMachine.AddAnyTransition(from, condition);
    public virtual void OnEnter()
    {
        SetUp(brain);
        //brain.AnimMaster.SetWeaponTarget(brain.player);
        brain.Animator.SetBool("Combat", true);
        //brain.AnimMaster.StartCoroutine(brain.AnimMaster.SetWeightOverTime(1f, .2f));
    }

    public virtual void OnExit()
    {
        //brain.AnimMaster.StopCoroutine(brain.AnimMaster.SetWeightOverTime(1f, .2f));
        //brain.AnimMaster.StartCoroutine(brain.AnimMaster.SetWeightOverTime(0f, .2f));

        brain.Animator.SetBool("Combat", false);
        //brain.AnimMaster.SetOriginalHeadTarget();
    }
    Vector3 aimOffset = new Vector3(0, 3f, 0);
    public virtual void Tick()
    {
        brain.AnimMaster.UpdateAimPosition(brain.player.position + aimOffset);
        if (!brain.Agent.pathPending && brain.Agent.remainingDistance <= brain.Agent.stoppingDistance && !brain.Agent.hasPath || brain.Agent.velocity.sqrMagnitude == 0f)
        {
            // Calculate the direction to the target
            Vector3 directionToTarget = (brain.player.position - brain.transform.position).normalized;
            directionToTarget.y = 0;
            // Create a rotation that looks in the direction of the target
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            // Rotate the agent towards the target rotation over time
            brain.transform.rotation = Quaternion.Slerp(brain.transform.rotation, lookRotation, Time.deltaTime * brain.Agent.angularSpeed);
        }
        stateMachine.Tick();
    }
    public Color GizmoColor()
    {
        return Color.red;
        //return stateMachine.GetGizmoColor();
    }
}
