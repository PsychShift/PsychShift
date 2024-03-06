using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class RagdollState : IState
{
    EnemyBrain brain;
    RigColliderManager rigColliderManager;
    TempGravity tempGravity;
    public bool IsDead;
    private bool hitGround;
    private const float onGroundForSeconds = 10f;
    
    float elapsedTime = 0;

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
        if(!hitGround && GroundedCheck())
        {
            hitGround = true;
            elapsedTime = 0;
            //rigColliderManager.PopulateBoneTransforms(rigColliderManager._ragdollBoneTransforms);
        }
        if(hitGround)
        {
            hitGround = GroundedCheck();
            if(elapsedTime > onGroundForSeconds)
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
            elapsedTime += Time.deltaTime;
        }
    }
    /* private void AlignRotationToHips()
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

    } */
    
    private static readonly Vector3 castDirection = Vector3.down;
    private const float castDistance = 3f;
    private static readonly Vector3 boxSize = new Vector3(1f, 0.1f, 1f);
    public bool GroundedCheck()
    {
        //Ragdoll=>turn off groundcheck=> standup => turn on check again
        RaycastHit[] hits = Physics.BoxCastAll(rigColliderManager.Rigidbodies[0].transform.position, boxSize, castDirection, Quaternion.identity, castDistance, EnemyBrain.groundLayer, QueryTriggerInteraction.Ignore);
        if(hits.Any(hit => hit.collider != null))
            return true;
        
        return false;
    }
    public Color GizmoColor()
    {
        return Color.black;
    }
}
