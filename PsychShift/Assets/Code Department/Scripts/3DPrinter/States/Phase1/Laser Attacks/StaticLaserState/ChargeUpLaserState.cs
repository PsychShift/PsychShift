using UnityEngine;

public class ChargeUpLaserState : IState
{
    private LaserShooter laser;
    private LaserBeamStats stats;

    float timer;
    private bool isFinished = false;
    public System.Func<bool> IsFinished => () => isFinished;

    public ChargeUpLaserState(LaserShooter laser, LaserBeamStats stats)
    {
        this.laser = laser;
        this.stats = stats;
    }

    public Color GizmoColor()
    {
        return Color.red;
    }

    public void OnEnter()
    {
        // play charge up particle effects
        isFinished = false;
        timer = 0f;
    }

    public void OnExit()
    {

    }

    public void Tick()
    {
        if(timer > stats.RestForTime)
        {
            isFinished = true;
            return;
        }
        timer += Time.deltaTime;
    }
}