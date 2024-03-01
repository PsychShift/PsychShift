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
        rigColliderManager._elapsedResetBonesTime = 0;
        rigColliderManager.EnableRagdoll();
    }

    public void OnExit()
    {
        //Debug.Log("Ragdoll exit");
        brain.Model.transform.parent = null;
        brain.CharacterInfo.controller.enabled = false;
        
        
        /* AlignRotationToHips();
        AlignPositionToHips(); */
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
            rigColliderManager.PopulateBoneTransforms(rigColliderManager._ragdollBoneTransforms);
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
                    //isDone = rigColliderManager.ResettingBonesBehavior();
                    isDone = true;
                }
            }
        }
    }
    private void AlignRotationToHips()
    {
        Vector3 originalHipsPosition = rigColliderManager._hipBones.position;
        Quaternion originalHipsRotation = rigColliderManager._hipBones.rotation;

        Vector3 desiredDirection = rigColliderManager._hipBones.up *-1;
        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(brain.transform.forward, desiredDirection);
        brain.transform.rotation *= fromToRotation;

        rigColliderManager._hipBones.position = originalHipsPosition;
        rigColliderManager._hipBones.rotation = originalHipsRotation;
    }
    private void AlignPositionToHips()
    {
        Vector3 originalHipsPosition = rigColliderManager._hipBones.position;
        brain.transform.position = rigColliderManager._hipBones.position;
        Vector3 positionOffset = rigColliderManager._standUpBoneTransforms[0].Position;
        positionOffset.y =0;
        positionOffset = brain.transform.rotation * positionOffset;
        brain.transform.position -= positionOffset;

        Debug.Log("Aligning");
        rigColliderManager._hipBones.position = originalHipsPosition;

    }
    

    public Color GizmoColor()
    {
        return Color.black;
    }
}
