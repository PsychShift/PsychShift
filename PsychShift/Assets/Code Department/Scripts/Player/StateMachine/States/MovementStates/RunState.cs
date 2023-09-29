using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public RunState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }
    public void Tick()
    {

    }

    public void OnEnter()
    {
        Debug.Log("Hello from Run");
        currentCharacter = playerStateMachine.currentCharacter;
    }

    public void OnExit()
    {
        
    }

    public void InitializeSubState()
    {
        
    }
}