using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetLocationState : IState
{
    EnemyBrain brain;
    Vector3 homePosition;

    public SetLocationState(EnemyBrain brain, Vector3 homePosition)
    {
        this.brain = brain;
        this.homePosition = homePosition;
    }
    public Color GizmoColor()
    {
        return Color.blue;
    }

    public void OnEnter()
    {
        brain.Agent.isStopped = false;
        Vector3 closestNavMeshPosition = GetClosestNavMeshPosition(homePosition);
        brain.Agent.SetDestination(closestNavMeshPosition);
        brain.Animator.SetFloat("speed", 1f);
    }

    public void OnExit()
    {
        brain.Agent.velocity = Vector3.zero;
        brain.Agent.isStopped = true;
        brain.Animator.SetFloat("speed", 0f);
    }

    public void Tick()
    {
        
    }

    public Func<bool> IsDone() => () => IsFinished();
    private bool IsFinished()
    {
        if (brain.Agent.remainingDistance < 0.5f)
        {
            return true;
        }

        return false;
    }

    private Vector3 GetClosestNavMeshPosition(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 1.0f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        else
        {
            Debug.LogError("No NavMesh found near the specified position.");
            return position; // Fallback to the original position if no NavMesh found
        }
    }
}
