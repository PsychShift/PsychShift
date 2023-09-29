using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunState : IState
{
    private IState currentSubState;
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public WallRunState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }

    
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        currentCharacter = playerStateMachine.currentCharacter;
    }

    public void OnExit()
    {
        
    }

    public void AddSubState(IState subState)
    {
        currentSubState = subState;
    }

    public IState GetCurrentSubState()
    {
        return currentSubState;
    }
}
