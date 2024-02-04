using System;
using System.Collections;
using UnityEngine;

public class RandomLocationState : IState
{
    public Vector3 Destination { get; set; }
    private EnemyBrain brain;
    float walkRadius = 30f;
    public RandomLocationState(EnemyBrain brain)
    {
        this.brain = brain;
    }
    float endAt = 0f;
    public void OnEnter()
    {
        PickRandomLocation();
        float time = brain.agression == null ? 1f : UnityEngine.Random.Range(brain.agression.WaitAroundTime.x, brain.agression.WaitAroundTime.y);
        endAt = Time.time + time;
        brain.Animator.SetFloat("speed", 1f);
    }

    public void OnExit()
    {
        brain.Animator.SetFloat("speed", 0f);
        /* brain.Animator.SetFloat("speedForward", 0f);
        brain.Animator.SetFloat("speedRight", 0f); */
    }

    public void Tick()
    {
        //AnimatorHelper.SetMovementVector(brain.Animator, brain.Agent.velocity, brain.Model, "speedForward", "speedRight");
    }

    public Color GizmoColor()
    {
        return Color.blue;
    }

    private void PickRandomLocation()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * walkRadius;
        randomDirection += brain.transform.position;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out UnityEngine.AI.NavMeshHit hit, walkRadius, 1);
        Vector3 finalPosition = hit.position;
        brain.CharacterInfo.agent.SetDestination(finalPosition);
    }

    public Func<bool> IsDone() => () => IsFinished();
    private bool IsFinished()
    {
        if(/* brain.CharacterInfo.agent.remainingDistance < 0.01f  || */Time.time > endAt)
        {
            return true;
        }

        return false;        
    }
}
