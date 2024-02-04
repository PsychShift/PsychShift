using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : IState
{

    EnemyBrain brain;
    public ReloadState(EnemyBrain brain)
    {
        this.brain = brain;
    }
    public void OnEnter()
    {
        brain.Animator.SetFloat("cover", 1);
        brain.Animator.SetTrigger("reload");
    }
    public void OnExit()
    {
        brain.Animator.SetFloat("cover", 0);
    }
    public void Tick()
    {
        throw new System.NotImplementedException();
    }
    public Color GizmoColor()
    {
        return Color.grey;
    }
}
