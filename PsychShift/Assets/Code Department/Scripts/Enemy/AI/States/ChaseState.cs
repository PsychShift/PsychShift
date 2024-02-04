using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : ShootingSuperState, ICoroutineRestarter
{
    Transform gunParent;

    public ChaseState(EnemyBrain brain)
    {
        this.brain = brain;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        brain.Agent.stoppingDistance = brain.agression.PlayerStoppingDistance;
        brain.StartCoroutine(ChasePlayer());
        brain.Animator.SetFloat("speed", 1f);
    }

    public override void OnExit()
    {
        base.OnExit();
        brain.Animator.SetFloat("speed", 0f);
        /* brain.Animator.SetFloat("speedForward", 0f);
        brain.Animator.SetFloat("speedRight", 0f); */
        brain.Agent.stoppingDistance = brain.agression.DestinationStoppingDistance;
        brain.StopCoroutine(ChasePlayer());
    }

    public void RestartCoroutine()
    {
        brain.StartCoroutine(ChasePlayer());
    }

    public override void Tick()
    {
        base.Tick();
        if(brain.Agent.remainingDistance < brain.Agent.stoppingDistance)
        {
            brain.Animator.SetFloat("speed", 0f);
            brain.Agent.isStopped = true;
            brain.Agent.velocity = Vector3.zero;
            /* brain.Animator.SetFloat("speedForward", 0f);
            brain.Animator.SetFloat("speedRight", 0f); */
        }
        else
        {
            brain.Animator.SetFloat("speed", 1f);
            brain.Agent.isStopped = false;
        }

        //AnimatorHelper.SetMovementVector(brain.Animator, brain.Agent.velocity, brain.Model, "speedForward", "speedRight");
        
        // aim at the player based on the gun forward
        /* Vector3 direction = brain.player.position - gunParent.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        gunParent.rotation = Quaternion.Lerp(gunParent.rotation, lookRotation, Time.deltaTime * 10f); */

    }

    private IEnumerator ChasePlayer()
    {
        while (true)
        {
            if(brain.Agent.isOnNavMesh)
            {
                brain.Agent.SetDestination(brain.player.transform.position);
                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
