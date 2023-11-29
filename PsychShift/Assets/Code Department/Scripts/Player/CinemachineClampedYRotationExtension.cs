using UnityEngine;
using Cinemachine;

public class CinemachineClampedYRotationExtension : CinemachineExtension
{
    public Vector3 baseRotationEulerAngles = Vector3.zero; // Base rotation to lock at (in Euler angles)
    public float maxLeftRotation = 45f;  // Maximum rotation to the left
    public float maxRightRotation = 45f; // Maximum rotation to the right

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            // Extract the euler angles from the base rotation
            Vector3 baseRotation = baseRotationEulerAngles;

            // Calculate the rotation difference from the base rotation
            Vector3 rotationDifference = state.RawOrientation.eulerAngles - baseRotation;

            // Clamp the y-rotation within the specified limits
            rotationDifference.y = Mathf.Clamp(rotationDifference.y, -maxLeftRotation, maxRightRotation);

            // Apply the clamped rotation back
            state.RawOrientation = Quaternion.Euler(baseRotation + rotationDifference);
        }
    }
}