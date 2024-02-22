using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollState : IState
{
    RigColliderManager rigColliderManager;
    public RagdollState(RigColliderManager rigColliderManager)
    {
        this.rigColliderManager = rigColliderManager;
    }
    public void OnEnter()
    {
        // activate the ragdoll
        rigColliderManager.EnableRagdoll();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }

    public Color GizmoColor()
    {
        return Color.black;
    }
}
