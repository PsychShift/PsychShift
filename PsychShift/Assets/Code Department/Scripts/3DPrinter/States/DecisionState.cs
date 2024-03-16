using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic in-between state. Gives time to decide the next action.
/// </summary>
public class DecisionState : IState
{
    private HangingRobotController controller;
    public DecisionState(HangingRobotController controller)
    {
        this.controller = controller;
    }

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
        controller.canMove = true;
    }

    public void OnExit()
    {
        controller.canMove = false;
    }

    public void Tick()
    {
        if(Time.time > endTime)
            isDone = true;
    }
}
