using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReloadState : IState
{

    EnemyBrain brain;
    AIAgression agression;
    public ReloadState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }
    public void OnEnter()
    {
        Debug.Log("Start Reload");
        brain.CharacterInfo.animator.SetFloat("cover", 1);
        brain.CharacterInfo.animator.SetTrigger("reload");
    }
    public void OnExit()
    {
        Debug.Log("stop reload");
        brain.CharacterInfo.animator.SetFloat("cover", 0);
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
