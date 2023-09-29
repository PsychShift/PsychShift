using UnityEngine;
public class GroundedState : RootState, IState
{
    private CharacterInfo currentCharacter;
    public GroundedState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
    {
        this.playerStateMachine = playerStateMachine;
        this.stateMachine = stateMachine;
    }
    
    public void Tick()
    {
        Look();
        HandleGravity();
        // Call the Tick method of the current sub-state
        SubStateTick();
    }

    public void OnEnter()
    {
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
        playerStateMachine.CurrentMovementY = playerStateMachine.gravityValue;
        playerStateMachine.AppliedMovementY = playerStateMachine.gravityValue;
    }
}