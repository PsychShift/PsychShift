using System;
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
        // sub state machine
        stateMachine = new StateMachine.StateMachine();
        var shootState = new ShootState(brain, agression);
        var delayState = new DelayState(1f);
        var reloadState = new ReloadState(brain, agression);

        // Transitions
        AT(shootState, reloadState,() => brain.CharacterInfo.gunHandler.ShouldReload());
        AT(reloadState, delayState, () => !brain.CharacterInfo.gunHandler.ShouldReload());
        AT(delayState, shootState, delayState.IsDone());

        stateMachine.SetState(shootState);

        void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState from, Func<bool> condition) => stateMachine.AddAnyTransition(from, condition);
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
