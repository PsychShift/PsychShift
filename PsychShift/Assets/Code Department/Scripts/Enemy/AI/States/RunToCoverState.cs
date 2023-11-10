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

    public void OnEnter()
    {
        currentCharacterInfo = brain.CharacterInfo;
        brain.currentCover = CoverArea.Instance.GetCover(brain.transform.position);
        currentCharacterInfo.agent.SetDestination(brain.currentCover.transform.position);
    }

    public void OnExit()
    {
        brain.currentCover = null;
    }

    public void Tick()
    {
        
    }
    public Color GizmoColor()
    {
        return Color.blue;
    }
}
