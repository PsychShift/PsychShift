using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingAnimatorController : MonoBehaviour
{
    Animator animator;
    [SerializeField] private HangingRobotController brain;
    public SpineControllerIK spineIK;
    public HangingRobotArmsIK armsController;

    public Transform playerTarget => brain.target;
    // add ik references to adjust the weights in other scripts, specifically the movement ones.

    public Vector3 Velocity { get { return brain.Velocity; } }
    private float spineRotAmount;

    /// <summary>
    /// When set, invoke the onSpineRotateRequest event, causing a state transition in the SpineControllerIK script.
    /// </summary> 
    public float SpineRotationAmount 
    { 
        get
        {
            return spineRotAmount;
        } 
        set
        {
            spineRotAmount = value;
            //spineIK.onSpineRotateRequest.Invoke();
        } 
    }

    private int staticTriggerHash;
    private int attackDoneHash;


    void Start()
    {
        animator = GetComponent<Animator>();
        staticTriggerHash = Animator.StringToHash("StaticShootTrigger");
        attackDoneHash = Animator.StringToHash("AttackDone");
    }

    public void RightArmAttack(bool on)
    {
        if(on)
            animator.SetTrigger(staticTriggerHash);
        else
            animator.SetTrigger(attackDoneHash);
    }

    [SerializeField] private float modelRotationDamping = 1f;
    public void RotateTowardsTarget(Vector3 targetPos)
    {
        Vector3 lookPos = targetPos - transform.position;

        Quaternion lookRot = Quaternion.LookRotation(lookPos, Vector3.up);

        float eulerY = lookRot.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(-90, 0, eulerY);


        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * modelRotationDamping);
    }

    public void SetArmAimingManual(bool manual)
    {
        if(manual)
        {

        }
        else
        {
            
        }
    }

    
}
