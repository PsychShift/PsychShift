using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A generic in-between state. Gives time to decide the next action.
/// </summary>
public class DecisionState : IState
{
    private HangingRobotController controller;

    public Queue<IState> stateQueue;
    public DecisionState(HangingRobotController controller)
    {
        this.controller = controller;
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
        if(stateQueue.Count > 0)
        {
            Debug.Log("Queued state: " + stateQueue.Peek());
            controller.attacksStateMachine.SetState(stateQueue.Dequeue());
        }
        else
        {
            int index = UnityEngine.Random.Range(0, controller.attackStates.Length);
            controller.attacksStateMachine.SetState(controller.attackStates[index]);
            Debug.Log("Randome state: " + controller.attackStates[index]);
        }
    }

    float waitForSeconds = 4f;
    float endTime = 0f;
    private bool isDone;
    public Func<bool> IsDone => () => isDone;

    public Color GizmoColor()
    {
        return Color.white;
    }

    public void OnEnter()
    {
        endTime = Time.time + waitForSeconds;
        controller.canMove = true;
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
