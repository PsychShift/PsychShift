using UnityEngine;
/// <summary>
/// Root state responsible for handling  gravity after reaching the peak of a jump or falling off a ledge.
/// </summary> 
public class FallState : RootState, IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public FallState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
    {
        this.playerStateMachine = playerStateMachine;
        this.stateMachine = stateMachine;
    }

    public void Tick()
    {
        HandleGravity();
        SubStateTick();
    }

    public void OnEnter()
    {
        Debug.Log("Hello from Fall");
        currentCharacter = playerStateMachine.currentCharacter;
        SetSubState();
    }

    public void OnExit()
    {
        stateMachine._currentSubState = currentSubState;
    }

    private void HandleGravity()
    {
        playerStateMachine.playerVelocity.y += playerStateMachine.gravityValue * Time.deltaTime;
        currentCharacter.controller.Move(playerStateMachine.playerVelocity * Time.deltaTime);
    }
}