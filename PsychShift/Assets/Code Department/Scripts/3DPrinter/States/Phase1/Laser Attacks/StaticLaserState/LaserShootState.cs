using UnityEngine;

public class LaserShootState : IState
{
    private StaticLaserShotState rootState;
    private LaserShooter laser;
    private LaserBeamStats stats;

    private bool isFinished = false;
    public System.Func<bool> IsFinished => () => isFinished;

    public LaserShootState(StaticLaserShotState rootState, LaserShooter laser, LaserBeamStats stats)
    {
        this.rootState = rootState;
        this.laser = laser;
        this.stats = stats;
    }

    public Color GizmoColor()
    {
        return Color.red;
    }

    public void OnEnter()
    {
        timer = 0;
        isFinished = false;
        rootState.controller.canRotate = false;
        laser.Fire(true, stats);
    }

    public void OnExit()
    {
        laser.CeaseFire();
        rootState.controller.canRotate = true;
        rootState.isFinished = true;
    }
    float timer;
    public void Tick()
    {
        if(timer > stats.ShootForTime)
        {
            isFinished = true;
            return;
        }
        timer += Time.deltaTime;
    }


}