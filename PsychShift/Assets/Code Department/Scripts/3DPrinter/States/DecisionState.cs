using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic in-between state. Gives time to decide the next action.
/// </summary>
[System.Serializable]
public class DecisionState : IState
{
    private HangingRobotController controller;
    private EnemyLauncher enemyLauncher;

    public Queue<IState> stateQueue;
    public float decisionTime = 4f;
    public float desiredY = -10;
    public DecisionState(HangingRobotController controller, EnemyLauncher enemyLauncher, float desiredY)
    {
        this.controller = controller;
        this.enemyLauncher = enemyLauncher;
        enemyLauncher.needMoreEnemies = true;
        this.desiredY = desiredY;

        stateQueue = new Queue<IState>();
    }




    // public method to add a state to swap to

    // when isDone, manually SetState() to the next item in queue
    // if the queue is empty pick a random state to enter
    


    public void AddStateToQueue(IState state)
    {
        stateQueue.Enqueue(state);
    }

    private void SetState()
    {
        Debug.Log(stateQueue.Count);
        if(stateQueue.Count > 0)
        {
            controller.attacksStateMachine.SetState(stateQueue.Dequeue());
        }
        else
        {
            int min = enemyLauncher.needMoreEnemies ? 0 : 1;
            int index = UnityEngine.Random.Range(min, controller.attackStates.Length);
            controller.attacksStateMachine.SetState(controller.attackStates[index]);
        }
    }

    float endTime = 0f;
    private bool isDone;
    public Func<bool> IsDone => () => isDone;

    public Color GizmoColor()
    {
        return Color.white;
    }

    public void OnEnter()
    {
        endTime = Time.time + decisionTime;
        controller.canMove = true;
        //controller.DesiredY = desiredY;
    }

    public void OnExit()
    {
        controller.canMove = false;
    }

    public void Tick()
    {
        if(Time.time > endTime)
        {
            SetState();
            isDone = true;
        }
    }
}
