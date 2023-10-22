using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowards : MonoBehaviour
{
    public Transform target; // The target object to rotate towards
    public float rotationSpeed = 5.0f; // Adjust the rotation speed as needed

    void Update()
    {
        if (target != null)
        {
            // Calculate the direction to the target
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Optionally, set the y-component to 0 if you don't want vertical rotation.

            if (direction != Vector3.zero)
            {
                // Rotate the object towards the target
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}