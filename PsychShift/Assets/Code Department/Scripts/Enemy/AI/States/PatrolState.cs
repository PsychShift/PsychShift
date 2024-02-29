using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : IState
{
    private EnemyBrain brain;
    private Player.CharacterInfo currentCharacterInfo;
    private Vector3[] patrolPoints;
    private int patrolPointsLength;
    private int wpIndex = 0;

    public PatrolState(EnemyBrain brain, Vector3[] patrolPoints)
    {
        this.brain = brain;
        currentCharacterInfo = brain.CharacterInfo;
        patrolPointsLength = patrolPoints.Length;

        for(int i = 0; i < patrolPointsLength; i++)
        {
            patrolPoints[i] = GetClosestNavMeshPosition(patrolPoints[i]);
        }
        this.patrolPoints = patrolPoints.Where(p => p.y != -99999).ToArray();
        patrolPoints = this.patrolPoints;
    }
    public PatrolState()
    {
        Debug.Log("Im just chillin");
    }

    public void OnEnter()
    {
        brain.Agent.isStopped = false;
        wpIndex = (wpIndex + 1) % patrolPointsLength;
        brain.Agent.SetDestination(patrolPoints[wpIndex]);
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

    public System.Func<bool> IsDone() => () => IsFinished();
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
            return new Vector3(0, -99999, 0); // Fallback to the original position if no NavMesh found
        }
    }

    public Color GizmoColor()
    {
        return Color.white;
    }
}

