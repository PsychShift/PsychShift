using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyState : IState
{
    public int spawnAmount = 10;
    private HangingRobotController hangingRobotController;
    private EnemyLauncher enemyLauncher;
    private HangingRobotArmsIK armsController;

    public SpawnEnemyState(HangingRobotController hangingRobotController, EnemyLauncher enemyLauncher, HangingRobotArmsIK armsController)
    {
        this.hangingRobotController = hangingRobotController;
        this.enemyLauncher = enemyLauncher;
        this.armsController = armsController;
    }

    public Color GizmoColor()
    {
        return Color.clear;
    }

    public void OnEnter()
    {
        for(int i = 0; i < spawnAmount; i++)
        {
            enemyLauncher.ShootEnemy();
        }
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        
    }
}
