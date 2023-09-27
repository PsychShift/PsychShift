using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardState : IState
{
    private readonly PlayerStateMachine _playerStateMachine;
    public StandardState(PlayerStateMachine stateMachine)
    {
        _playerStateMachine = stateMachine;
    }
    public void Tick()
    {

    }
    public void OnEnter()
    {

    }
    public void OnExit()
    {

    }
}
