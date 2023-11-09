using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToCoverState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;

    public RunToCoverState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }
    
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }
    public Color GizmoColor()
    {
        throw new System.NotImplementedException();
    }
}
