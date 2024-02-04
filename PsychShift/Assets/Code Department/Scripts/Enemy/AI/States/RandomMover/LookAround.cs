using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundState : IState
{
    EnemyBrain brain;

    public LookAroundState(EnemyBrain brain)
    {
        this.brain = brain;
    }

    float endAt = 0f;
    public void OnEnter()
    {
        float time = brain.agression == null ? 1f : UnityEngine.Random.Range(brain.agression.WaitAroundTime.x, brain.agression.WaitAroundTime.y);
        endAt = Time.time + time;
        brain.Agent.velocity = Vector3.zero;
        brain.Agent.isStopped = true;
        brain.Animator.SetFloat("speed", 0f);
        /* brain.Animator.SetFloat("speedForward", 0f);
        brain.Animator.SetFloat("speedRight", 0f); */
    }

    public void OnExit()
    {
        brain.Agent.isStopped = false;
    }

    public void Tick()
    {
        
    }

    private void LookAround()
    {
        // set animator bool "combat" to true
    }

    public Func<bool> IsDone() => () => IsFinished();
    private bool IsFinished()
    {
        if(/* brain.CharacterInfo.agent.remainingDistance < 0.5f || */ Time.time > endAt)
        {
            return true;
        }

        return false;        
    }

    public Color GizmoColor()
    {
        return Color.yellow;
    }
}
