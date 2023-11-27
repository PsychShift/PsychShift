using UnityEngine;
using Cinemachine;
using System.Collections;

[AddComponentMenu("")] // Hide this script from the component menu
[ExecuteInEditMode]
public class CinemachineTiltExtension : CinemachineExtension
{
    public float tiltSpeed = 75f;
    private float tiltDirection = 0f; // 1 for right, -1 for left, 0 for no tilt

    private float previousTiltAngle = 0f;
    float targetAngle = 0f;

    // You may add more parameters as needed

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            // Apply the tilt to the camera
            previousTiltAngle = Mathf.Lerp(previousTiltAngle, targetAngle, Time.deltaTime * tiltSpeed);

            state.Lens.Dutch = previousTiltAngle;
        }
    }

    float CalculateTiltAngle()
    {
        // Implement your tilt angle calculation logic here
        // You can use input, follow targets, or other criteria to determine the tilt angle
        return tiltDirection * Mathf.Sin(Time.time * tiltSpeed) * 30.0f; // Example: Tilt back and forth
    }

    
    public void SetTiltDirection(float direction)
    {
        targetAngle = direction;
    }
}