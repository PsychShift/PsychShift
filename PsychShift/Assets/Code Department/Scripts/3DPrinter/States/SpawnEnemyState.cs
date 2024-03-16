using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnEnemyState : IState
{
    public int spawnAmount = 1;
    private HangingRobotController hangingRobotController;
    private EnemyLauncher enemyLauncher;
    private HangingRobotArmsIK armsController;

    float endTime = 0f;
    private bool isDone;
    public Func<bool> IsDone => () => isDone;

    public SpawnEnemyState(HangingRobotController hangingRobotController, EnemyLauncher enemyLauncher, HangingRobotArmsIK armsController)
    {
        this.hangingRobotController = hangingRobotController;
        this.enemyLauncher = enemyLauncher;
        this.armsController = armsController;
    }

    public Color GizmoColor()
    {
        return Color.blue;
    }

    public void OnEnter()
    {
        endTime = Time.time + spawnAmount * .5f;
        hangingRobotController.canMove = true;
        enemyLauncher.StartCoroutine(enemyLauncher.ShootEnemies(spawnAmount, 0.5f, hangingRobotController.guns, hangingRobotController.modifiers));
    }

    public void OnExit()
    {
        hangingRobotController.canMove = false;
    }

    public void Tick()
    {
        if (Time.time > endTime)
            isDone = true;
    }
}
