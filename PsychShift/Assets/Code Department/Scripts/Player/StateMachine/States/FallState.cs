using UnityEngine;
/// <summary>
/// Root state responsible for handling  gravity after reaching the peak of a jump or falling off a ledge.
/// </summary> 
public class FallState : RootState, IState
{
    private CharacterInfo currentCharacter;
    public FallState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
    {
        this.playerStateMachine = playerStateMachine;
        this.stateMachine = stateMachine;
    }

    public void Tick()
    {
        Look();
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
        float previousYVelocity = playerStateMachine.CurrentMovementY;
        playerStateMachine.CurrentMovementY = playerStateMachine.CurrentMovementY + playerStateMachine.gravityValue * Time.deltaTime;
        playerStateMachine.AppliedMovementY = Mathf.Max((previousYVelocity + playerStateMachine.CurrentMovementY) * .5f, -20f);
    }
}