using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StaticLaserShotState : IState
{
    public LaserShooter laser;
    public LaserBeamStats stats;
    public float desiredY;

    [HideInInspector] public HangingRobotController controller;
    private HangingAnimatorController animController;
    private HangingRobotArmsIK armsController;
    private SlowAimAtTargetState aimState;
        
    public StaticLaserShotState(HangingRobotController controller, HangingAnimatorController animController, HangingRobotArmsIK armsController, LaserShooter laser, LaserBeamStats stats, float desiredY)
    {
        this.controller = controller;
        this.animController = animController;
        this.armsController = armsController;
        this.laser = laser;
        this.stats = stats;
        this.desiredY = desiredY;

        // TO DO - Make a state machine that does each part in steps
        stateMachine = new StateMachine.StateMachine();


        // 1 aiming, slowly aim at the player, show a red beam for conveince
        aimState = new SlowAimAtTargetState(controller, armsController);

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

    [HideInInspector] public bool isFinished = false;


    bool started = false;
    public void OnEnter()
    {
        isFinished = false;
        started = false;
        stateMachine.SetState(aimState);
        //controller.DesiredY = desiredY;
        //controller.StartCoroutine(controller.WaitForHeight(stateMachine, aimState, () => Start()));
        Start();
        //controller.TurnOnNeckIK(false, 0.25f);
        animController.RightArmAttack(true);
    }

    public void OnExit()
    {
        armsController.SetRightArmManualOverTime(false, 0.6f);
        animController.RightArmAttack(false);
        //controller.TurnOnNeckIK(true, 0.25f);
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
        armsController.SetRightArmManualOverTime(true, 0.1f);
        started = true;
    }

    public Func<bool> IsFinished => () => isFinished;
}

