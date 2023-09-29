using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public CrouchState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }
    public void Tick()
    {

    }

    public void OnEnter()
    {
        Debug.Log("Hello from Crouch");
        currentCharacter = playerStateMachine.currentCharacter;
    }

    public void OnExit()
    {
        
    }

    public void InitializeSubState()
    {
        
    }
}