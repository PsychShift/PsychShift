using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;//Kevin added this

public class CinemachinePOVExtension : CinemachineExtension
{
    [SerializeField]
    private float horizontalSpeed = 10f;
    [SerializeField]
    private float verticalSpeed = 10f;
    [SerializeField]
    private float clampAngle = 80f;
    private InputManager inputManager;
    private Vector3 startingRotation;

    //Detect form of input
    Gamepad gamepad;
    Keyboard keyboard;
    Mouse mouse;

    protected override void Awake()
    {
        gamepad = Gamepad.current;
        keyboard = Keyboard.current;
        mouse = Mouse.current;
        inputManager = InputManager.Instance;
        if(startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
        base.Awake();
    }
    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            if(stage == CinemachineCore.Stage.Aim)
            {
                Vector2 deltaInput = inputManager.GetMouseDelta();
                if(gamepad!=null)
                {
                    horizontalSpeed*= 9;
                    verticalSpeed*= 9;
                }
                startingRotation.x += deltaInput.x * horizontalSpeed * Time.deltaTime;
                startingRotation.y += deltaInput.y * verticalSpeed * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, startingRotation.x, 0f);
            }
        }
    }
}