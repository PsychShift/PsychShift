using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveStateMB : IState
{
    HangingRobotController controller;
    HangingAnimatorController animController;
    public MoveStateMB(HangingRobotController controller, HangingAnimatorController animController)
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

    }

    public void OnExit()
    {

    }

    public void Tick()
    {

    }
}
