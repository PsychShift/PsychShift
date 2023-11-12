using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TempGravity : MonoBehaviour
{
    Vector3 appliedMovement;
    float CurrentMovementY;
    float AppliedMovementY;

    float gravityValue = -100f;
    CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float previousYVelocity = CurrentMovementY;
        CurrentMovementY = CurrentMovementY + gravityValue * Time.deltaTime;
        AppliedMovementY = Mathf.Max((previousYVelocity + CurrentMovementY) * .5f, -20f);
        appliedMovement.y = AppliedMovementY;

        controller.Move(appliedMovement * Time.deltaTime);
    }
}
