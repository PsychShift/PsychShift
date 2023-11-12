using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSuperState : IState
{
    protected StateMachine.StateMachine stateMachine;
    protected EnemyBrain brain;
    protected AIAgression agression;
    protected void SetUp(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
        stateMachine = new StateMachine.StateMachine();

        var idleShootState = new IdleShootState(brain, agression);
        var activeShootState = new ActiveShootState(brain, agression);
        var reloadState = new ReloadState(brain, agression);

        // Transitions
        AT(idleShootState, activeShootState, idleShootState.IsDone());
        AT(activeShootState, idleShootState, activeShootState.IsDone());
        AT(activeShootState, reloadState,() => brain.CharacterInfo.gunHandler.ShouldReload());
        AT(reloadState, activeShootState, () => !brain.CharacterInfo.gunHandler.ShouldReload());


        stateMachine.SetState(activeShootState);


    }
    protected void AT(IState from, IState to, Func<bool> condition) => stateMachine.AddTransition(from, to, condition);
    protected void Any(IState from, Func<bool> condition) => stateMachine.AddAnyTransition(from, condition);

    public virtual void OnEnter()
    {
        Debug.Log($"{this} is a ShootingSuperState");
        SetUp(brain, agression);
    }

    public virtual void OnExit()
    {
        
    }

    public virtual void Tick()
    {
        stateMachine.Tick();
    }
    public Color GizmoColor()
    {
        return stateMachine.GetGizmoColor();
    }
}
