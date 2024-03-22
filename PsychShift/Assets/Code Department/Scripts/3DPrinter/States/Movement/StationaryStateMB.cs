using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StationaryStateMB : IState
{
    HangingRobotController controller;
    HangingAnimatorController animController;

    private float climbSpeed = 1f;
    public StationaryStateMB(HangingRobotController controller, HangingAnimatorController animController)
    {
        this.controller = controller;
        this.animController = animController;
    }
    public Color GizmoColor()
    {
        return Color.white;
    }

    public void OnEnter()
    {
        controller.agent.isStopped = true;
        Debug.Log("Stationary State");
    }

    public void OnExit()
    {
        controller.agent.isStopped = false;
    }

    public void Tick()
    {
        
    }

    /*private void ControlHeight()
    {
        Vector3 localPos = controller.model.position;
        if (localPos.y + 5f > controller.desiredY)
        {
            localPos.y -= climbSpeed * Time.deltaTime;
            controller.model.position = localPos;
        }
        else if(localPos.y - 5f < controller.desiredY)
        {
            localPos.y += climbSpeed * Time.deltaTime;
            controller.model.position = localPos;
        }
        Debug.Log(controller.model.name + ": " + localPos + " " + controller.model.position);
        Debug.Log($"{controller.desiredY} {controller.model.position.y}");
    }*/
}
