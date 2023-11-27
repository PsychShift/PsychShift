using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnimatorHelper
{
    public static void SetMovementVector(Animator animator, Vector3 movementVector, Transform lookTransform, string vertical, string horizontal)
    {
        Vector3 appliedVector = lookTransform.InverseTransformDirection(movementVector);
        float speedForward = Mathf.Clamp01(appliedVector.z);
        float speedRight = Mathf.Clamp01(appliedVector.x);
        
        animator.SetFloat(vertical, speedForward);
        animator.SetFloat(horizontal, speedRight);
    }
}
