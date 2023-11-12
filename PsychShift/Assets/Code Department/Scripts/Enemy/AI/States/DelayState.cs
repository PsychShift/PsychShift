using System;
using UnityEngine;

public class DelayState : IState
{
    float waitForSeconds;
    private float deadline;
    public DelayState(float waitForSeconds)
    {
        this.waitForSeconds = waitForSeconds;
    }

    public Color GizmoColor()
    {
        return Color.white;
    }

    public void OnEnter()
    {
        Debug.Log("DelayState OnEnter");
        deadline = Time.time + waitForSeconds;
    }

    public void OnExit()
    {
        Debug.Log("DelayState OnExit");
    }

    public void Tick()
    {
        
    }

    private bool DelayOver()
    {
        return Time.time >= deadline;
    }

    public Func<bool> IsDone() => () => DelayOver();
}
