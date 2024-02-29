using UnityEngine;

public class SlowAimAtTargetState : IState
{

    private bool isFinished = false;
    private HangingRobotArmsIK armsController;

    public System.Func<bool> IsFinished => () => isFinished;

    private float aimForSeconds = 2f;
    public Transform target;

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
    }

    public void OnExit()
    {
        
    }

    float timer = 0f;
    Vector3 startPos;
    public void Tick()
    {
        if(timer > aimForSeconds)
        {
            isFinished = true;
            return;
        }
        // do stuff
        Vector3 moveTo = Vector3.Lerp(startPos, target.position, timer/aimForSeconds);
        armsController.AimLeftHandTarget(moveTo);
        timer += Time.deltaTime;
    }
}