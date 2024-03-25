using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HangingRobotArmsIK : MonoBehaviour
{
    [Header("Left Arm")]
    public Transform leftArmTarget;
    public Transform leftArmShoulderBone;
    public Transform leftHandBone;
    //[SerializeField] private TwoBoneIKConstraint leftArmBoneIK;
    [SerializeField] private TwoBoneIKConstraint leftArmIK;
    [SerializeField] private MultiRotationConstraint leftArmRotationIK;


    [Header("Right Arm")]
    public Transform rightArmTarget;
    public Transform rightHandBone;
    [SerializeField] private TwoBoneIKConstraint rightArmIK;
    [SerializeField] private MultiRotationConstraint rightArmRotationIK;


    [Header("Spine")]
    [Tooltip("The spine transform that the arms are attached to.")]
    [SerializeField] private Transform spine;
    [SerializeField] private float handsDistanceFromSpine = 5f;
    

    public void SetLeftArmManualOverTime(bool manual, float transitionTime)
    {
        if(manual)
        {
            StartCoroutine(LerpFloatOverTime(0, 1, transitionTime, (result) => leftArmIK.weight = result));
            StartCoroutine(LerpFloatOverTime(0, 1, transitionTime, (result) => leftArmRotationIK.weight = result));
        }
        else
        {
            StartCoroutine(LerpFloatOverTime(1, 0, transitionTime, (result) => leftArmIK.weight = result));
            StartCoroutine(LerpFloatOverTime(1, 0, transitionTime, (result) => leftArmRotationIK.weight = result));
        }
        /* if(manual)
        {
        }
        else
        {
        } */
    }
    public void SetRightArmManualOverTime(bool manual, float transitionTime)
    {
        if(manual)
        {
            StartCoroutine(RightLerpFloatOverTime(0, 1, transitionTime));
            //StartCoroutine(LerpFloatOverTime(0, 1, transitionTime, (result) => rightArmRotationIK.weight = result));
        }
        else
        {
            StartCoroutine(RightLerpFloatOverTime(1, 0, transitionTime));
            //StartCoroutine(LerpFloatOverTime(1, 0, transitionTime, (result) => rightArmRotationIK.weight = result));
        }
        /* if(manual)
        {
        }
        else
        {
        } */
    }

    
    public void AimLeftHandTarget(Vector3 targetPos)
    {
        // Find a point in-between the target position and the spine near the shoulder

        // Get the direction from the spine, to the target.
        /*Vector3 direction = targetPos - leftArmShoulderBone.position;
        direction.Normalize();

        // Set the up value of the target to the direction
        leftArmTarget.up = direction;
        float dist = Vector3.Distance(leftHandBone.position, leftArmShoulderBone.position);
        // Set the position of the arm to be the spine position, plus the direction times some value
        leftArmTarget.position = leftArmShoulderBone.position + (direction * dist);*/
        leftArmTarget.position = targetPos;
    }
    public void AimRightHandTarget(Vector3 targetPos)
    {
        /* // Find a point in-between the target position and the spine near the shoulder

        // Get the direction from the spine, to the target.
        Vector3 direction = targetPos - spine.position;
        direction.Normalize();

        // Set the up value of the target to the direction
        rightArmTarget.up = direction;
        float dist = Vector3.Distance(rightHandBone.position, spine.position);
        // Set the position of the arm to be the spine position, plusthe direction times some value
        rightArmTarget.position = spine.position + (direction * dist); */
        rightArmTarget.position = targetPos;
        
    }

    IEnumerator LerpFloatOverTime(float startValue, float endValue, float duration, System.Action<float> resultCallback)
    {
        float startTime = Time.time;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float currentLerp = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            Debug.Log(currentLerp);
            resultCallback.Invoke(currentLerp);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        resultCallback.Invoke(endValue);
    }
    IEnumerator RightLerpFloatOverTime(float startValue, float endValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float currentLerp = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            rightArmRotationIK.weight = currentLerp;
            rightArmIK.weight = currentLerp;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        rightArmRotationIK.weight = endValue;
        rightArmIK.weight = endValue;
    }
}
