using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLocationState : IState
{
    EnemyBrain brain;
    Vector3 homePosition;

    public SetLocationState(EnemyBrain brain, Vector3 homePosition)
    {
        this.brain = brain;
        this.homePosition = homePosition;
    }
    public Color GizmoColor()
    {
        return Color.blue;
    }

    public void OnEnter()
    {
        brain.Agent.isStopped = false;
        brain.Agent.SetDestination(homePosition);
        brain.Animator.SetFloat("speed", 1f);
    }

    public void OnExit()
    {
        brain.Agent.velocity = Vector3.zero;
        brain.Agent.isStopped = true;
        brain.Animator.SetFloat("speed", 0f);
    }

    public void Tick()
    {
        
    }

    public Func<bool> IsDone() => () => IsFinished();
    private bool IsFinished()
    {
        if (brain.Agent.remainingDistance < 0.1f)
        {
            return true;
        }

        return false;
    }
}
