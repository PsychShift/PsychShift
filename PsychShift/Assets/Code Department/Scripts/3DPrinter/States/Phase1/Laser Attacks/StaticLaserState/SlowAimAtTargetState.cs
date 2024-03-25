using UnityEngine;

public class SlowAimAtTargetState : IState
{
    private bool isFinished = false;
    private HangingRobotController controller;
    private HangingRobotArmsIK armsController;
    private float aimForSeconds = 2f;
    public Transform target;

    private Vector3 startPos;
    private Vector3 localTargetPos;
    float timer = 0f;

    public System.Func<bool> IsFinished => () => isFinished;

    public SlowAimAtTargetState(HangingRobotController controller, HangingRobotArmsIK armsController)
    {
        this.controller = controller;
        this.armsController = armsController;
    }

    public Color GizmoColor()
    {
        return Color.white;
    }

    public void OnEnter()
    {
        timer = 0;
        isFinished = false;
        armsController.rightArmTarget.position = armsController.rightHandBone.position;
        startPos = armsController.rightArmTarget.position;

        // Convert the target position to the body's local space
        
        localTargetPos = armsController.transform.InverseTransformPoint(controller.target.position);
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        if(timer > aimForSeconds)
        {
            isFinished = true;
            return;
        }

        // Interpolate between the start position and the local target position
        Vector3 moveTo = Vector3.Lerp(startPos, armsController.transform.TransformPoint(localTargetPos), timer/aimForSeconds);
        armsController.AimRightHandTarget(moveTo);
        timer += Time.deltaTime;
    }
}