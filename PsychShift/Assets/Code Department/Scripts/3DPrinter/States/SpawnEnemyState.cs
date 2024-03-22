using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class SpawnEnemyState : IState
{
    public EnemyLauncher enemyLauncher;
    public int spawnAmount = 1;
    public float desiredY;

    private HangingRobotController controller;
    private HangingRobotArmsIK armsController;

    float endTime = 0f;
    private bool isDone;
    public Func<bool> IsDone => () => isDone;

    bool started = false;

    public SpawnEnemyState(HangingRobotController controller, EnemyLauncher enemyLauncher, HangingRobotArmsIK armsController, float desiredY, int spawnAmount)
    {
        this.controller = controller;
        this.enemyLauncher = enemyLauncher;
        this.armsController = armsController;
        this.desiredY = desiredY;
        this.spawnAmount = spawnAmount;
    }

    public Color GizmoColor()
    {
        return Color.blue;
    }

    public void OnEnter()
    {
        controller.canMove = true;
        started = false;
        controller.DesiredY = desiredY;
        controller.StartCoroutine(controller.WaitForHeight(() => Start()));
        controller.TurnOnNeckIK(false, 0.25f);
    }

    public void OnExit()
    {
        controller.canMove = false;
        controller.TurnOnNeckIK(true, 0.25f);
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
