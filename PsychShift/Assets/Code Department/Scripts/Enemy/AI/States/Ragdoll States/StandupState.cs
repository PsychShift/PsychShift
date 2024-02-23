using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandupState : IState
{
    RigColliderManager rigColliderManager;
    EnemyBrain brain;
    public bool isStanding = false;
    public StandupState(EnemyBrain brain, RigColliderManager rigColliderManager)
    {
        this.brain = brain;
        this.rigColliderManager = rigColliderManager;
    }
    public void OnEnter()
    {
        rigColliderManager.EnableAnimator();
    }

    public void OnExit()
    {
        brain.Agent.isStopped = true;
        isStanding = true;
    }

    public void Tick()
    {
        
    }

    public Color GizmoColor()
    {
        return Color.white;
    }
}
