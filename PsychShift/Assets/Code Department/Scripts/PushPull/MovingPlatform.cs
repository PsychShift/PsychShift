using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class MovingPlatform : MonoBehaviour
{
    protected Vector3 movementVector;
    void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterController controller))
        {
            //controller.transform.parent = transform;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out CharacterController controller))
        {
            //controller.transform.parent = transform;
            if(other.tag == "Player")
            {
                PlayerStateMachine.Instance.ExternalMovement = movementVector;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerStateMachine.Instance.ExternalMovement = Vector3.zero;
        }
    }
}
