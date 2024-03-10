using UnityEngine;

public class SlowAimAtTargetState : IState
{
    private bool isFinished = false;
    private HangingRobotArmsIK armsController;
    private float aimForSeconds = 2f;
    public Transform target;

    private Vector3 startPos;
    private Vector3 localTargetPos;

    public System.Func<bool> IsFinished => () => isFinished;

    public SlowAimAtTargetState(HangingRobotArmsIK armsController)
    {
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
        armsController.leftArmTarget.position = armsController.leftHandBone.position;
        startPos = armsController.leftArmTarget.position;

        // Convert the target position to the body's local space
        localTargetPos = armsController.transform.InverseTransformPoint(target.position);
    }

    public void OnExit()
    {
        
    }

    float timer = 0f;
    public void Tick()
    {
        if(timer > aimForSeconds)
        {
            isFinished = true;
            return;
        }

        // Interpolate between the start position and the local target position
        Vector3 moveTo = Vector3.Lerp(startPos, armsController.transform.TransformPoint(localTargetPos), timer/aimForSeconds);
        armsController.AimLeftHandTarget(moveTo);
        timer += Time.deltaTime;
    }
}