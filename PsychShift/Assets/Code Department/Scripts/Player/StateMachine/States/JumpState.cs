using UnityEngine;
/// <summary>
/// Root state, responsible for making an enemy Jump and handling gravity in air.
/// </summary>
public class JumpState : RootState, IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    
    public JumpState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
    {
        this.playerStateMachine = playerStateMachine;
        this.stateMachine = stateMachine;
    }
    
    public void Tick()
    {
        HandleGravity();
        // Call the Tick method of the current sub-state
        SubStateTick();
    }

    public void OnEnter()
    {
        Debug.Log("Hello from Jump");
        currentCharacter = playerStateMachine.currentCharacter;
        playerStateMachine.playerVelocity.y += Mathf.Sqrt(playerStateMachine.jumpHeight * -3.0f * playerStateMachine.gravityValue);
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