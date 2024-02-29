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
        Debug.Log("laser shoot");
        timer = 0;
        isFinished = false;
        laser.Fire(true, stats);
    }

    public void OnExit()
    {
        laser.CeaseFire();
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