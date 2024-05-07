using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;//Kevin added this

public class CinemachinePOVExtension : CinemachineExtension
{
    public static float horizontalSpeed = 10f;
    public static float verticalSpeed = 10f;

    [SerializeField]
    private float clampAngle = 80f;
    private Vector3 startingRotation;

    protected override void Awake()
    {
        if(startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
        base.Awake();
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = InputManager.Instance.GetMouseDelta();
                startingRotation = Vector3.Lerp(startingRotation, new Vector3 (startingRotation.x + deltaInput.x, startingRotation.y + deltaInput.y, 0), 2 * deltaTime);
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
    }
}