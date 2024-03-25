using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

[System.Serializable]
public class RotatingLaserState : IState
{
    public LaserShooter laser;
    public LaserBeamStats stats;
    public float desiredY;

    private HangingRobotController controller;
    private HangingAnimatorController animController;
    private StingerController stingerController;
    private readonly System.Random rand;
    public Func<bool> IsDone => () => stingerController.isDone;
    public RotatingLaserState(HangingRobotController controller, HangingAnimatorController animController, StingerController stingerController, LaserShooter laser, LaserBeamStats stats, float desiredY)
    {
        this.controller = controller;
        this.animController = animController;
        this.stingerController = stingerController;
        this.laser = laser;
        this.stats = stats;
        this.desiredY = desiredY;
        stingerController.laserShooter.defaultStats = stats;

        rand = new System.Random();
    }
    public Color GizmoColor()
    {
        return Color.black;
    }

    public void OnEnter()
    {
        //controller.DesiredY = desiredY;
        //controller.StartCoroutine(controller.WaitForHeight(() => Start()));
        Start();
        
    }

    public void OnExit()
    {
        //stingerController.laserShooter.CeaseFire();
        controller.TurnOnNeckIK(true, 0.25f);
    }

    public void Tick()
    {
        
    }

    void Start()
    {
        controller.TurnOnNeckIK(false, 0.25f);
        //stingerController.laserShooter.Fire(true);
        bool randomDir = rand.NextDouble() > 0.5;
        stingerController.FireLaser(randomDir);
    }
}

