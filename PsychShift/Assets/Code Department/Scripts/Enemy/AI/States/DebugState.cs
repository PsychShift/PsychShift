using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugState : IState
{
    public Color GizmoColor()
    {
        // Create an RGB effect that shifts colors over time.
        float r = Mathf.Sin(Time.time * 2f) * 0.5f + 0.5f;
        float g = Mathf.Sin(Time.time * 2f + 2f) * 0.5f + 0.5f;
        float b = Mathf.Sin(Time.time * 2f + 4f) * 0.5f + 0.5f;
        return new Color(r, g, b);
    }

    public void OnEnter()
    {
        Debug.Log("Enter DebugState. We just chillin here");
    }

    public void OnExit()
    {
        Debug.Log("Exit DebugState. We no longer chillin here");
    }

    public void Tick()
    {
        
    }
}
