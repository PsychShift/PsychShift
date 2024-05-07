using UnityEngine;
using Cinemachine;

[AddComponentMenu("")] // Hide this script from the component menu
[ExecuteInEditMode]
public class CinemachineRecoilExtension : CinemachineExtension
{
    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Noise)
        {
            state.OrientationCorrection = Quaternion.Euler(GunRecoil.Instance.rot);
        }
    }
}
