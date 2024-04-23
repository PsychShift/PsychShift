using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetLocationState : IState
{
    EnemyBrain brain;
    Vector3 homePosition;
    float time = 0;
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
        Debug.Log("entering set location state");
        brain.Agent.isStopped = false;
        Vector3 closestNavMeshPosition = GetClosestNavMeshPosition(homePosition);
        /* GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = closestNavMeshPosition; */
        brain.Agent.SetDestination(closestNavMeshPosition);
        brain.Animator.SetFloat("speed", 1f);
    }

    public void OnExit()
    {
        Debug.Log("exiting set location state");
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
        time += Time.deltaTime;
        if (time > 1f && brain.Agent.remainingDistance < 0.5f)
        {
            Debug.Log(brain.gameObject.name + " is moving to destination");
            return true;
        }

        return false;
    }

    private Vector3 GetClosestNavMeshPosition(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
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
