using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollState : IState
{
    RigColliderManager rigColliderManager;
    EnemyBrain brain;
    public bool IsDead;
    private bool hitGround;
    private float onGroundForSeconds = 10f;
    float endTime = 0;

    public bool isDone = false;
    public Func<bool> IsDone => () => isDone;
    public RagdollState(EnemyBrain brain, RigColliderManager rigColliderManager)
    {
        this.brain = brain;
        this.rigColliderManager = rigColliderManager;
    }
    public void OnEnter()
    {
        Debug.Log("raggidy");
        // activate the ragdoll
        hitGround = false;
        isDone = false;
        brain.Agent.isStopped = false;
        rigColliderManager.EnableRagdoll();
    }

    public void OnExit()
    {
        //Turn everything back on once enemy is standing again
        //brain.Agent.isStopped = true;
    }

    public void Tick()
    {
        if(!hitGround && GroundedCheck())
        {
            hitGround = true;
            endTime = Time.time + onGroundForSeconds;
        }
        if(hitGround)
        {
            if(Time.time > endTime)
            {
                // timer is done, should I stand or die???
                if(IsDead)
                {
                    brain.DestroyMe();
                }
                else
                {
                    //call rig collider 
                    //don't exit until stand anim is done
                    //sfadfvsdafsad
                    isDone = true;
                    //brain.RagdollNotDead();
                }
            }
        }
    }

    public Color GizmoColor()
    {
        return Color.black;
    }
    LayerMask groundLayer = ~0;
    Vector3 castDirection = Vector3.down;
    float castDistance = 0.0f;
    Vector3 boxSize = new Vector3(1f, 0.1f, 1f);//GROUND KEVIN CHANGE .4F .1F,.4F Old nums
    private bool GroundedCheck()
    {
        RaycastHit[] hits = Physics.BoxCastAll(brain.transform.position, boxSize, castDirection, Quaternion.identity, castDistance, groundLayer, QueryTriggerInteraction.Ignore);
        if(hits.Any(hit => hit.collider != null))
            return true;
        
        return false;
    }
}
