using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandupState : IState
{
    RigColliderManager rigColliderManager;
    public StandupState(RigColliderManager rigColliderManager)
    {
        this.rigColliderManager = rigColliderManager;
    }
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }

    public Color GizmoColor()
    {
        return Color.white;
    }
}
