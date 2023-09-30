using UnityEngine;

public class SlowState : RootState, IState
{
    private CharacterInfo currentCharacter;
    
    public SlowState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
    {
        this.playerStateMachine = playerStateMachine;
        this.stateMachine = stateMachine;
    }
    public void OnEnter()
    {
        SetSubState();
        Debug.Log("Hello From Slow State");
        playerStateMachine.SwapControlMap(true);
        TimeManager.Instance.DoSlowmotion(0.1f);
    }

    public void OnExit()
    {
        TimeManager.Instance.UndoSlowmotion();
    }

    public void Tick()
    {
        Look();

        SubStateTick();
    }
}
