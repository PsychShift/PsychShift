using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunToCoverState : IState
{
    private EnemyBrain brain;
    private AIAgression agression;
    private Player.CharacterInfo currentCharacterInfo;
    public RunToCoverState(EnemyBrain brain, AIAgression agression)
    {
        this.brain = brain;
        this.agression = agression;
    }
    Transform target;

    public void OnEnter()
    {
        currentCharacterInfo = brain.CharacterInfo;
    }

    public void OnExit()
    {
        brain.currentCover = null;
        target = null;
        brain.CharacterInfo.agent.ResetPath();
    }

    public void Tick()
    {
        if(target == null)
        {
            target = brain.FindCover().transform;
            if(target != null)
                currentCharacterInfo.agent.SetDestination(target.position);
        }

    }
    public Color GizmoColor()
    {
        return Color.blue;
    }
}
