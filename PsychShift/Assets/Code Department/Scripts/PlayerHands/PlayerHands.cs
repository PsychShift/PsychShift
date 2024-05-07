using System.Collections;
using System.Collections.Generic;
using Guns;
using Player;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerHands : MonoBehaviour
{
    [Header("Animation")]
    public static PlayerHands Instance;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private TwoBoneIKConstraint leftIK;
    [SerializeField] private TwoBoneIKConstraint rightIK;
    [Header("Camera Movement")]
    [SerializeField] private float moveAmount = 0.1f;
    [SerializeField] private Vector3 moveMaxSway = Vector3.one * 2;
    [SerializeField] private float moveSmoothAmount = 6;
    private Transform gunTransform;
    private Vector3 initialPosition;

    [Header("Walk Movement")]
    [SerializeField] private float walkMoveAmount = 3;
    [SerializeField] private float walkMoveSmoothAmount = 0.2f;


    [Header("Camera Rotation")]
    [SerializeField] private float rotationAmount = 10;
    [SerializeField] private Vector3 maxRotationAmount = Vector3.one * 3;
    [SerializeField] private float smoothRotationAmount = 1;

    [Header("Walk Rotation")]
    [SerializeField] private float walkRotationAmount = 5;
    [SerializeField] private float walkSmoothRotationAmount = 5;

    [Space(20)]
    [SerializeField] private bool rotX = true;
    [SerializeField] private bool rotY = false;
    [SerializeField] private bool rotZ = true;

    private Quaternion initialRotation;
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    Vector2 mouseMovement;
    Vector2 walkMovement;
    void Update()
    {
        mouseMovement = InputManager.Instance.GetMouseDelta() * moveAmount;
        walkMovement = InputManager.Instance.MoveVector * moveAmount;
        CameraMoveSway();
        CameraTiltSway();
        WalkMoveSway();
        WalkTiltSway();
    }

    private void CameraMoveSway()
    {
        float moveX = Mathf.Clamp(mouseMovement.x, -moveMaxSway.x, moveMaxSway.x) * moveAmount;
        float moveY = Mathf.Clamp(mouseMovement.y, -moveMaxSway.y, moveMaxSway.y) * moveAmount;

        Vector3 finalPos = new Vector3(moveX, moveY, 0);
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, finalPos + initialPosition, Time.deltaTime * moveSmoothAmount);
    }

    private void CameraTiltSway()
    {
        float tiltX = rotX ? -Mathf.Clamp(mouseMovement.x, -maxRotationAmount.x, maxRotationAmount.x) * rotationAmount : 0;
        float tiltY = rotY ? Mathf.Clamp(mouseMovement.y, -maxRotationAmount.y, maxRotationAmount.y) * rotationAmount : 0;
        float tiltZ = rotZ ? Mathf.Clamp(mouseMovement.y, -maxRotationAmount.z, maxRotationAmount.z) * rotationAmount : 0;

        Quaternion finalRot = Quaternion.Euler(tiltX, tiltY, tiltZ);
        gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, finalRot * initialRotation, Time.deltaTime * smoothRotationAmount);
    }

    private void WalkMoveSway()
    {
        
        float moveX = Mathf.Clamp(PlayerStateMachine.Instance.AppliedMovementX, -moveMaxSway.x, moveMaxSway.x) * walkMoveAmount;
        float moveY = -Mathf.Clamp(PlayerStateMachine.Instance.AppliedMovementY, -moveMaxSway.y, moveMaxSway.y) * walkMoveAmount;
        float moveZ = Mathf.Clamp(PlayerStateMachine.Instance.AppliedMovementZ, -moveMaxSway.z, moveMaxSway.z) * walkMoveAmount;

        Vector3 finalPos = new Vector3(moveX, moveY, moveZ);
        gunTransform.localPosition = Vector3.Lerp(gunTransform.localPosition, finalPos + initialPosition, Time.deltaTime * walkMoveSmoothAmount);
    }

    private void WalkTiltSway()
    {
        float tiltX = rotX ? -Mathf.Clamp(walkMovement.x, -maxRotationAmount.x, maxRotationAmount.x) * walkRotationAmount : 0;
        float tiltY = rotY ? Mathf.Clamp(walkMovement.y, -maxRotationAmount.y, maxRotationAmount.y) * walkRotationAmount: 0;
        float tiltZ = rotZ ? Mathf.Clamp(walkMovement.y, -maxRotationAmount.z, maxRotationAmount.z) * walkRotationAmount: 0;

        Quaternion finalRot = Quaternion.Euler(tiltX, tiltY, tiltZ);
        gunTransform.localRotation = Quaternion.Slerp(gunTransform.localRotation, finalRot * initialRotation, Time.deltaTime * walkSmoothRotationAmount);
    }
    public void SetHandPositions(GunScriptableObject gunSO)
    {
        gunTransform = gunSO.Model.transform;
        initialPosition = gunTransform.localPosition;
        initialRotation = gunTransform.localRotation;


        HandsOrientation handsOrientation = gunSO.GetHandOrientations()[0];
        leftIK.data.target = handsOrientation.leftHand.transform;
        leftIK.data.hint = handsOrientation.leftElbow.transform;
        rightIK.data.target = handsOrientation.rightHand.transform;
        rightIK.data.hint = handsOrientation.rightElbow.transform;

        rigBuilder.Build();
    }
}
