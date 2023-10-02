using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardState : RootState, IState
{
    private CharacterInfo currentCharacter;
    public StandardState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
    {
        this.playerStateMachine = playerStateMachine;
        this.stateMachine = stateMachine;
    }
    public void OnEnter()
    {
        Debug.Log("Hello From Standard State");
        SetSubState();
        
        InputManager.Instance.SwapControlMap(ActionMapEnum.standard);
        TimeManager.Instance.UndoSlowmotion();
    }

    public void OnExit()
    {
        
    }

    public void Tick()
    {
        Look();

        SubStateTick();
    }
}