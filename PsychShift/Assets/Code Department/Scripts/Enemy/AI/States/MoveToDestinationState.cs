using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToDestinationState : IState
{
    private bool useVector3Reference = false;
    private EnemyBrain brain;
    private AIAgression agression;
    private System.Func<Vector3> getDestination;
    public Vector3 Destination => getDestination.Invoke();
    public MoveToDestinationState(EnemyBrain brain, AIAgression agression, System.Func<Vector3> getDestination)
    {
        useVector3Reference = true;
        this.brain = brain;
        this.agression = agression;
        this.getDestination = getDestination;
    }
    public MoveToDestinationState(EnemyBrain brain, AIAgression agression)
    {
        useVector3Reference = false;
        this.brain = brain;
        this.agression = agression;
    }
    public void OnEnter()
    {
        if(useVector3Reference)
            brain.Agent.SetDestination(Destination);
        brain.Agent.isStopped = false;
    }

    public void OnExit()
    {
        brain.Agent.isStopped = false;
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
