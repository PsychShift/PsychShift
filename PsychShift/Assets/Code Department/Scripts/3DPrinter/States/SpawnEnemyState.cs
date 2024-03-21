using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.Universal;

public class SpawnEnemyState : IState
{
    public int spawnAmount = 1;
    private HangingRobotController controller;
    private EnemyLauncher enemyLauncher;
    private HangingRobotArmsIK armsController;

    float endTime = 0f;
    private bool isDone;
    public Func<bool> IsDone => () => isDone;

    bool started = false;

    public SpawnEnemyState(HangingRobotController controller, EnemyLauncher enemyLauncher, HangingRobotArmsIK armsController)
    {
        this.controller = controller;
        this.enemyLauncher = enemyLauncher;
        this.armsController = armsController;
    }

    public Color GizmoColor()
    {
        return Color.blue;
    }

    public void OnEnter()
    {
        controller.canMove = true;
        started = false;
        controller.DesiredY = -25f;
        controller.StartCoroutine(controller.WaitForHeight(() => Start()));
    }

    public void OnExit()
    {
        controller.canMove = false;
    }

    public void Tick()
    {
        if (started && Time.time > endTime)
            isDone = true;
    }

    void Start()
    {
        endTime = Time.time + spawnAmount * .5f;
        enemyLauncher.StartCoroutine(enemyLauncher.ShootEnemies(spawnAmount, 0.5f, controller.guns, controller.modifiers));
        started = true;
    }
}
