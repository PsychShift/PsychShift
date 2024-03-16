using System;
using UnityEngine;

public class RotatingLaserState : IState
{
    private readonly HangingRobotController controller;
    private readonly HangingAnimatorController animController;
    private readonly StingerController stingerController;
    private readonly LaserShooter laser;
    private readonly LaserBeamStats stats;
    private readonly System.Random rand;
    public Func<bool> IsDone => () => stingerController.isDone;
    public RotatingLaserState(HangingRobotController controller, HangingAnimatorController animController, StingerController stingerController, LaserShooter laser, LaserBeamStats stats)
    {
        this.controller = controller;
        this.animController = animController;
        this.stingerController = stingerController;
        this.laser = laser;
        this.stats = stats;
        stingerController.laserShooter.defaultStats = stats;
        rand = new System.Random();
    }
    public Color GizmoColor()
    {
        return Color.black;
    }

    public void OnEnter()
    {
        bool randomDir = rand.NextDouble() > 0.5;
        //stingerController.laserShooter.Fire(true);
        stingerController.FireLaser(randomDir);
    }

    public void OnExit()
    {
        //stingerController.laserShooter.CeaseFire();
    }

    public void Tick()
    {
        
    }
}

