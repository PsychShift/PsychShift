using System.Collections;
using System.Collections.Generic;
using Guns.Demo;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyAnimatorMaster : MonoBehaviour
{
    private EnemyGunSelector gunSelector;
    public Animator animator;
    public WeaponIKEnemy weaponIK;

    [Header("Rigs")]
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private GameObject armRigGameObject;
    [SerializeField] private GameObject headRigGameObject;
    
    [HideInInspector] public Rig armsRig;
    [HideInInspector] public Rig headRig;

    [Header("Head")]
    private GameObject headGO;
    [HideInInspector] public MultiAimConstraint headConstraint;
    private Transform originalHeadTarget;

    [Header("Right Arm")]
    private GameObject rightArmGO;
    [HideInInspector] public TwoBoneIKConstraint rightArmConstraint;

    [Header("Left Arm")]
    private GameObject leftArmGO;
    [HideInInspector] public TwoBoneIKConstraint leftArmConstraint;

    private void OnEnable()
    {
        GetRigComponents();
        GunSetup();
    }
    void OnDisable()
    {
        gunSelector.OnActiveGunSet -= PrepareAnimator;
    }

    public void GunSetup()
    {
        gunSelector = GetComponentInParent<EnemyGunSelector>();
        if(gunSelector == null)
        {
            Debug.LogError("Couldn't find the EnemyGunSelector script for the model: " + gameObject.name);
            return;
        }

        gunSelector.OnActiveGunSet += PrepareAnimator;      
    }
    
    public void PrepareAnimator()
    {
        List<HandsOrientation> orientations = gunSelector.ActiveGun.GetHandOrientations();
        HandsOrientation ori = orientations[0];
        SetHandPositions(ori.leftHand.transform, ori.rightHand.transform, ori.leftElbow.transform, ori.rightElbow.transform);
        SetWeaponAim(gunSelector.ActiveGun.ShootSystem.transform);
        weaponIK.ready = true;
    }

    public void GetRigComponents()
    {
        if(!armRigGameObject.TryGetComponent(out armsRig))
        {
            Debug.Log("Arm rig not found for the model: " + gameObject.name);
            return;
        }
        if(!gameObject.TryGetComponent(out weaponIK))
        {
            Debug.LogError("Couldn't find the WeaponIKEnemy script for the model: " + gameObject.name);
            return;
        }

        if(!headRigGameObject.TryGetComponent(out headRig))
        {
            Debug.Log("Head rig not found for the model: " + gameObject.name);
            return;
        }

        headGO = headRigGameObject.transform.Find("Head").gameObject;
        if(headGO == null)
        {
            Debug.Log("Head GameObject not found for the model: " + gameObject.name);
            return;
        }
        if(!headGO.TryGetComponent(out headConstraint))
        {
            Debug.LogError("Couldn't find the head Multi Aim Constraint for the model: " + gameObject.name);
            return;
        }
        originalHeadTarget = headConstraint.data.sourceObjects[0].transform;

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

    public void SetHandPositions(Transform leftHandTarget, Transform rightHandTarget, Transform leftElbowTarget, Transform rightElbowTarget)
    {
        // Set left hand target
        leftArmConstraint.data.target = leftHandTarget;
        // Set right hand target
        rightArmConstraint.data.target = rightHandTarget;
        // Set left elbow target
        leftArmConstraint.data.hint = leftElbowTarget;
        // Set right elbow target
        rightArmConstraint.data.hint = rightElbowTarget;
        rigBuilder.Build();
    }
    public void SetWeaponAim(Transform aim)
    {
        weaponIK.SetAimTransform(aim);
    }

    public void SetWeaponTarget(Transform aimTarget)
    {
        SetHeadTarget(aimTarget);

        weaponIK.SetTargetTransform(aimTarget);
    }

    public void SetHeadTarget(Transform headTarget)
    {
        headConstraint.data.sourceObjects.Clear();
        headConstraint.data.sourceObjects.Add(new WeightedTransform(headTarget, 1f));
    }
    public void SetOriginalHeadTarget()
    {
        headConstraint.data.sourceObjects.Clear();
        headConstraint.data.sourceObjects.Add(new WeightedTransform(originalHeadTarget, 1f));
    }


    public void AimAtTarget(bool isAiming)
    {
        if(isAiming)
        {
            headRig.weight = 1f;
        }
        else
        {
            headRig.weight = 0f;
        }
    }

    public void WeaponAim()
    {
        //weaponIK.Aim();
    }
}
