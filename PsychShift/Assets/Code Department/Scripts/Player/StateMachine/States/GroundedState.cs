using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public GroundedState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }
    public void Tick()
    {
        
    }

    public void OnEnter()
    {
        Debug.Log("Hello from Grounded");
        currentCharacter = playerStateMachine.currentCharacter;
    }

    public void OnExit()
    {
        
    }


    public void InitializeSubState()
    {
        
    }
}