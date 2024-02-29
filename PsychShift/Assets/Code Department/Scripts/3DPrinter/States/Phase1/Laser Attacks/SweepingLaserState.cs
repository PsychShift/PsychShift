using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweepingLaserState : IState
{
    private readonly HangingRobotController controller;
    private readonly HangingAnimatorController animController;
    private readonly HangingRobotArmsIK armsController;
    private readonly LaserShooter laser;
    private readonly LaserBeamStats stats;

    private readonly SlowAimAtTargetState aimState;
    private StateMachine.StateMachine stateMachine;
    public SweepingLaserState(HangingRobotController controller, HangingAnimatorController animController, HangingRobotArmsIK armsController, LaserShooter laser, LaserBeamStats stats)
    {
        this.controller = controller;
        this.animController = animController;
        this.armsController = armsController;
        this.laser = laser;
        this.stats = stats;

        // TO DO - Make a state machine that does each part in steps
        stateMachine = new StateMachine.StateMachine();

        void AT(IState to, IState from, System.Func<bool> condition) => stateMachine.AddTransition(to, from, condition);
    }
    public Color GizmoColor()
    {
        throw new System.NotImplementedException();
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }
}
