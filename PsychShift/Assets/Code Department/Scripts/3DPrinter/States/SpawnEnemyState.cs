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
    public int maxNumOfEnemies = 5;
    public int spawnAmount = 1;
    public float desiredY;

    private HangingRobotController controller;
    private HangingRobotArmsIK armsController;

    float endTime = 0f;
    private bool isDone;
    public Func<bool> IsDone => () => isDone;

    bool started = false;

    private Vector3 startPos;
    private Vector3 localTargetPos;
    float timer = 0f;
    float aimForSeconds = 1f;

    public SpawnEnemyState(HangingRobotController controller, EnemyLauncher enemyLauncher, HangingRobotArmsIK armsController, float desiredY, int spawnAmount, int maxNumOfEnemies)
    {
        this.controller = controller;
        this.enemyLauncher = enemyLauncher;
        this.enemyLauncher.maxNumOfEnemies = maxNumOfEnemies;
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
        //controller.DesiredY = desiredY;
        //controller.StartCoroutine(controller.WaitForHeight(() => Start()));
        Start();
        controller.animController.RightArmAttack(true);
        
        //controller.TurnOnNeckIK(false, 0.25f);        
    }

    public void OnExit()
    {
        controller.canMove = false;
        controller.animController.RightArmAttack(false);
        armsController.SetRightArmManualOverTime(false, 0.6f);
        //controller.TurnOnNeckIK(true, 0.25f);
    }

    public void Tick()
    {
        if(!started) return;
        
        if (Time.time > endTime)
            isDone = true;

        Vector3 moveTo = Vector3.Lerp(startPos, armsController.transform.TransformPoint(localTargetPos), timer/aimForSeconds);
        armsController.AimRightHandTarget(moveTo);
        timer += Time.deltaTime;
    }

    void Start()
    {
        armsController.SetRightArmManualOverTime(true, 0.1f);
        endTime = Time.time + spawnAmount * .5f;
        enemyLauncher.StartCoroutine(enemyLauncher.ShootEnemies(spawnAmount, 0.5f, controller.guns, controller.modifiers));
        started = true;

        armsController.rightArmTarget.position = armsController.rightHandBone.position;
        startPos = armsController.rightArmTarget.position;

        // Convert the target position to the body's local space
        localTargetPos = armsController.transform.InverseTransformPoint(controller.target.position);
    }
}
