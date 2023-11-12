using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAroundState : IState
{
    EnemyBrain brain;
    AIAgression agression;

    public LookAroundState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }

    float endAt = 0f;
    public void OnEnter()
    {
        endAt = Time.time + 3f;
        brain.CharacterInfo.agent.isStopped = true;
    }

    public void OnExit()
    {
        brain.CharacterInfo.agent.isStopped = false;
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
        if(brain.CharacterInfo.agent.remainingDistance < 0.5f || Time.time > endAt)
        {
            return true;
        }

        return false;        
    }

    public Color GizmoColor()
    {
        return Color.blue;
    }


}
