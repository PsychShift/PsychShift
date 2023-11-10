using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtCoverState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;
    private StateMachine.StateMachine stateMachine;

    public AtCoverState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
        // TODO: Another state machine
    }
    public void OnEnter()
    {
        Debug.Log("AtCoverState OnEnter");
        // set animator bool "combat" to true
    }

    public void OnExit()
    {
        // set animator bool "combat" to false
    }

    public void Tick()
    {
        
    }
    public Color GizmoColor()
    {
        return Color.gray;
    }
}
