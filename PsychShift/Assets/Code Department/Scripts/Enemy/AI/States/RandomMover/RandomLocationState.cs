using System;
using UnityEngine;

public class RandomLocationState : IState
{

    private EnemyBrain brain;
    private AIAgression agression;
    float walkRadius = 30f;
    public RandomLocationState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }
    float endAt = 0f;
    public void OnEnter()
    {
        PickRandomLocation();
        endAt = Time.time + 5f;
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }

    public Color GizmoColor()
    {
        return Color.blue;
    }

    private void PickRandomLocation()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += brain.transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        brain.CharacterInfo.agent.SetDestination(finalPosition);
    }

    public Func<bool> IsDone() => () => IsFinished();
    private bool IsFinished()
    {
        if(brain.CharacterInfo.agent.remainingDistance < 0.5f || Time.time > endAt)
        {
            return true;
        }

        return false;        
    }
}
