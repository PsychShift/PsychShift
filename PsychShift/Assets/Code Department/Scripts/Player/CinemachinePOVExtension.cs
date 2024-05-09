using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    public static float speed = 10f;

    [SerializeField]
    private float clampAngle = 80f;
    private Vector2 startingRotation;
    
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
                startingRotation.x += deltaInput.x * speed * Time.deltaTime;
                startingRotation.y += deltaInput.y * speed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
    }
    /* protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                // Apply the new orientation to the camera state
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
    }

    private void Update() 
    {
        Vector2 deltaInput = InputManager.Instance.GetMouseDelta() * speed;


        // Calculate the rate based on deltaTime to make the interpolation frame rate independent
        float rate = -(1 / Time.deltaTime) * Mathf.Log10(0.9f); // Adjusted to use deltaTime directly
        
        // Improved interpolation using the exponential function
        startingRotation = Vector2.Lerp(startingRotation, new Vector2(startingRotation.x + deltaInput.x, startingRotation.y + deltaInput.y), Mathf.Exp(-rate * Time.deltaTime));

        // Clamp the rotation to prevent it from going beyond the allowed angles
        startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
    } */
}