using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic in-between state. Gives time to decide the next action.
/// </summary>
public class DecisionState : IState
{
    float waitForSeconds = 4f;
    float endTime = 0f;
    private bool isDone;
    public Func<bool> IsDone => () => isDone;

    public Color GizmoColor()
    {
        return Color.white;
    }

    public void OnEnter()
    {
        endTime = Time.time + waitForSeconds;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if(Time.time > endTime)
            isDone = true;
    }
}
