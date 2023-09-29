using UnityEngine;
public class GroundedState : RootState, IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public GroundedState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
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
        playerStateMachine.playerVelocity.y = playerStateMachine.gravityValue * Time.deltaTime;
        Debug.Log("Hello from Grounded");
        currentCharacter = playerStateMachine.currentCharacter;
        SetSubState();
    }

    public void OnExit()
    {
        stateMachine._currentSubState = currentSubState;
    }

    private void HandleGravity()
    {
        playerStateMachine.move = new Vector3(playerStateMachine.move.x, -1, playerStateMachine.move.z);
        currentCharacter.controller.Move(playerStateMachine.move * Time.deltaTime);
    }
}