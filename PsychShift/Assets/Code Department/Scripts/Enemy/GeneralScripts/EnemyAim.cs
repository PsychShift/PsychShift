using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAim
{
    private GameObject gunModel;
    private Vector3 defaultRotation;
    private MonoBehaviour monoBehaviour;


    private Transform currentTarget;
    private Transform enemyModel;

    private float rotationSpeed = 5f;
    public EnemyAim(GameObject gunModel, MonoBehaviour monoBehaviour, Transform enemyTransform)
    {
        this.gunModel = gunModel;
        defaultRotation = gunModel.transform.localEulerAngles;
        this.monoBehaviour = monoBehaviour;
        enemyModel = enemyTransform;
    }
    

    public void SetTarget(Transform target)
    {
        currentTarget = target;
    }

    public void Aim()
    {
            // Get the direction to the player
            Vector3 directionToPlayer = currentTarget.position - enemyModel.position;

            /* // Rotate the enemy towards the player on the horizontal plane
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            enemyModel.rotation = Quaternion.Slerp(enemyModel.rotation, targetRotation, rotationSpeed * Time.deltaTime); */

            // Calculate the rotation only for the vertical axis
            Vector3 directionToPlayerVertical = currentTarget.position - gunModel.transform.position;
            Quaternion targetRotationVertical = Quaternion.LookRotation(directionToPlayerVertical);

            // Clamp the angle
            float clampedAngle = Mathf.Clamp(targetRotationVertical.eulerAngles.x, -80f, 80f);

            // Apply rotation only for up and down aiming on the x-axis
            gunModel.transform.rotation = Quaternion.Euler(clampedAngle, 0f, 0f);
        }

    // in a while loop, reset the rotation of the gun to the default rotation
    public IEnumerator ResetAim()
    {
        while (gunModel.transform.localEulerAngles != defaultRotation)
        {
            gunModel.transform.localEulerAngles = Vector3.Lerp(gunModel.transform.localEulerAngles, defaultRotation, 0.1f);
            yield return null;
        }
    }

    public float GetTargetRotation()
    {
        // get the direction to the target
        Vector3 direction = currentTarget.position - gunModel.transform.position;
        // get the angle between the direction and the forward of the gun
        float angle = Vector3.Angle(direction, gunModel.transform.forward);
        // get the cross product of the direction and the forward of the gun
        Vector3 cross = Vector3.Cross(direction, gunModel.transform.forward);
        // if the cross is pointing up, the angle is negative
        if (cross.y < 0) angle = -angle;
        // return the angle
        return angle;
    }
    
}
