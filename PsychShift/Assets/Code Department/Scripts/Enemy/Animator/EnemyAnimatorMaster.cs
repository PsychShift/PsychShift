using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAnimatorMaster : MonoBehaviour
{
    public Animator animator;
    [Header("Rigs")]
    [SerializeField] private GameObject armRigGameObject;
    [HideInInspector] public Rig armsRig;
    [HideInInspector] public Rig headRig;


    [Header("Right Arm")]
    private GameObject rightArmGO;
    [HideInInspector] public TwoBoneIKConstraint rightArmConstraint;

    [Header("Left Arm")]
    private GameObject leftArmGO;
    [HideInInspector] public TwoBoneIKConstraint leftArmConstraint;

    private void OnEnable()
    {
        if(!armRigGameObject.TryGetComponent(out armsRig))
        {
            Debug.Log("Arm rig not found for the model: " + gameObject.name);
            return;
        }

        rightArmGO = armRigGameObject.transform.Find("RightArm").gameObject;
        if(rightArmGO == null)
        {
            Debug.Log("Right Arm GameObject not found for the model: " + gameObject.name);
            return;
        }
        if(!rightArmGO.TryGetComponent(out  rightArmConstraint))
        {
            Debug.LogError("Couldn't find the right arm Two Bone IK Constraint for the model: " + gameObject.name);
            return;
        }

        leftArmGO = armRigGameObject.transform.Find("LeftArm").gameObject;
        if (leftArmGO == null)
        {
            Debug.Log("Left Arm GameObject not found for the model: " + gameObject.name);
            return;
        }
        if (!leftArmGO.TryGetComponent(out leftArmConstraint))
        {
            Debug.LogError("Couldn't find the right arm Two Bone IK Constraint for the model: " + gameObject.name);
            return;
        }
    }

    public void SetArmRigWeight(float weight)
    {
        armsRig.weight = weight;
    }
    public void SetHeadRigWeight(float weight)
    {
        headRig.weight = weight;
    }

    public void SetHandPositions(Transform leftHandTarget, Transform rightHandTarget)
    {
        // Set left hand target
        leftArmConstraint.data.target = leftHandTarget;
        // Set right hand target
        rightArmConstraint.data.target = rightHandTarget;
    }
}
