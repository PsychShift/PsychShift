using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangingAnimatorController : MonoBehaviour
{
    Animator animator;
    [SerializeField] private HangingRobotController brain;
    [SerializeField] private SpineControllerIK spineIK;
    public HangingRobotArmsIK armsController;

    public Transform playerTarget => brain.target;

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


    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("Curl");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            animator.Play("UnCurl");
        }
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
