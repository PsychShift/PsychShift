using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AI;

public class GuardState : IState
{
    private EnemyBrain brain;
    private Vector3 startPosition;
    private float rotationSpeed;

    private Player.CharacterInfo currentCharacterInfo;
    public GuardState(EnemyBrain brain, Vector3 startPosition)
    {
        this.brain = brain;
        this.startPosition = startPosition;
    }

    public void OnEnter()
    {
        currentCharacterInfo = brain.CharacterInfo;
        currentCharacterInfo.agent.SetDestination(startPosition);
        brain.Agent.enabled = false;
    }

    public void OnExit()
    {
        currentCharacterInfo.agent.enabled = true;
    }

    public void Tick()
    {

    }

    public Color GizmoColor()
    {
        return Color.black;
    }
}
