using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLaserShotState : IState
{
    private readonly HangingRobotController controller;
    private readonly HangingAnimatorController animController;
    private readonly HangingRobotArmsIK armsController;
    private readonly LaserShooter laser;
    private readonly LaserBeamStats stats;

    private readonly SlowAimAtTargetState aimState;
    
    //ChargeUpLaserState chargeLaserState;
    public StaticLaserShotState(HangingRobotController controller, HangingAnimatorController animController, HangingRobotArmsIK armsController, LaserShooter laser, LaserBeamStats stats)
    {
        this.controller = controller;
        this.animController = animController;
        this.armsController = armsController;
        this.laser = laser;
        this.stats = stats;

        // TO DO - Make a state machine that does each part in steps
        stateMachine = new StateMachine.StateMachine();


        // 1 aiming, slowly aim at the player, show a red beam for conveince
        aimState = new SlowAimAtTargetState(armsController);

        // 2 charge up, charge the laser
        var chargeLaserState = new ChargeUpLaserState(laser, stats);

        // 3, fire, shoot laser for x time
        var laserShootState = new LaserShootState(this, laser, stats);

        // 4 end, tell this state is finished
        var doNothing = new DoNothing();

        void AT(IState to, IState from, Func<bool> condition) => stateMachine.AddTransition(to, from, condition);

        AT(aimState, chargeLaserState, aimState.IsFinished);
        AT(chargeLaserState, laserShootState, chargeLaserState.IsFinished);
        AT(laserShootState, doNothing, laserShootState.IsFinished);
    }

    private readonly StateMachine.StateMachine stateMachine;

    public bool isFinished = false;


    bool started = false;
    public void OnEnter()
    {
        isFinished = false;
        started = false;
        //stateMachine.SetState(aimState);
        controller.DesiredY = -30f;
        controller.StartCoroutine(controller.WaitForHeight(stateMachine, aimState, () => Start()));
    }

    public void OnExit()
    {
        armsController.SetLeftArmManualOverTime(false, 0.6f);
    }

    public void Tick()
    {
        if (!started) return;
        stateMachine.Tick();
    }

    public Color GizmoColor()
    {
        return Color.red;
    }

    void Start()
    {
        armsController.SetLeftArmManualOverTime(true, 0.1f);
        aimState.target = controller.target;
        started = true;
    }

    public Func<bool> IsFinished => () => isFinished;
}

