using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public IdleState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }
    public void Tick()
    {

    }

    public void OnEnter()
    {
        Debug.Log("Hello from Idle");
        currentCharacter = playerStateMachine.currentCharacter;
    }

    public void OnExit()
    {

    }

    public void InitializeSubState()
    {

    }
}
