using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RagdollState : IState
{
    RigColliderManager rigColliderManager;
    EnemyBrain brain;
    TempGravity tempGravity;
    public bool IsDead;
    private bool hitGround;
    private float onGroundForSeconds = 10f;
    
    float endTime = 0;

    public bool isDone = false;
    public Func<bool> IsDone => () => isDone;
    public RagdollState(EnemyBrain brain, RigColliderManager rigColliderManager, TempGravity tempGravity)
    {
        this.brain = brain;
        this.rigColliderManager = rigColliderManager;
        this.tempGravity = tempGravity;
    }
    public void OnEnter()
    {
        //Debug.Log("raggidy");
        // activate the ragdoll
        hitGround = false;
        isDone = false;
        brain.Agent.isStopped = true;
        brain.Agent.enabled = false;
        tempGravity.enabled = true;
        rigColliderManager.EnableRagdoll();
    }

    public void OnExit()
    {
        //Debug.Log("Ragdoll exit");
        brain.Model.transform.parent = null;
        brain.CharacterInfo.controller.enabled = false;
        brain.transform.position = brain.Model.transform.GetChild(1).position;
        brain.CharacterInfo.controller.enabled = true;
        brain.Model.transform.parent = brain.transform;
        tempGravity.enabled = false;
    }

    public void Tick()
    {
        if(!hitGround && brain.GroundedCheck())
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
                    brain.CharacterInfo.controller.enabled = false;
                    isDone = true;
                }
            }
        }
    }

    public Color GizmoColor()
    {
        return Color.black;
    }
}
