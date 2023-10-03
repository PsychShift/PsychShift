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
        SubStateTick();
        HandleGravity();
    }

    public void OnEnter()
    {
        Debug.Log("Hello from Fall");
        currentCharacter = playerStateMachine.currentCharacter;
/*         if(playerStateMachine.InAirForward == Vector3.zero)
        {
            playerStateMachine.InAirForward = currentCharacter.model.transform.forward;
            playerStateMachine.InAirRight = currentCharacter.model.transform.right;
        } */
        SetSubState();
    }

    public void OnExit()
    {
        stateMachine._currentSubState = currentSubState;
    }

    private void HandleGravity()
    {
        float previousYVelocity = playerStateMachine.CurrentMovementY;
        playerStateMachine.CurrentMovementY = playerStateMachine.CurrentMovementY + playerStateMachine.gravityValue * 8f * Time.deltaTime;
        playerStateMachine.AppliedMovementY = Mathf.Max((previousYVelocity + playerStateMachine.CurrentMovementY) * .5f, -20f);
    }
}