using UnityEngine;
/// <summary>
/// Root state, responsible for making an enemy Jump and handling gravity in air.
/// </summary>
public class JumpState : RootState, IState
{
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
        //Debug.Log("Hello from Jump");
        currentCharacter = playerStateMachine.currentCharacter;
        playerStateMachine.InAirForward = currentCharacter.model.transform.forward;
        playerStateMachine.InAirRight = currentCharacter.model.transform.right;
        HandleJump();
        SetSubState();
    }

    public void OnExit()
    {
        stateMachine._currentSubState = currentSubState;
    }

    private void HandleJump()
    {
        playerStateMachine.CurrentMovementY = playerStateMachine.InitialJumpVelocity;
        playerStateMachine.AppliedMovementY = playerStateMachine.InitialJumpVelocity;
    }

    private void HandleGravity()
    {
        bool isFalling = playerStateMachine.CurrentMovementY <= 0f || InputManager.Instance.jumpAction.triggered;
        float fallMultiplier = 2.0f;

        if(isFalling)
        {
            float previousYVelocity = playerStateMachine.CurrentMovementY;
            playerStateMachine.CurrentMovementY = playerStateMachine.CurrentMovementY + (playerStateMachine.InitialJumpGravity * fallMultiplier * Time.deltaTime);
            playerStateMachine.AppliedMovementY = Mathf.Max((previousYVelocity + playerStateMachine.CurrentMovementY) * .5f, -20f);
        }
        else
        {
            float previousYVelocity = playerStateMachine.CurrentMovementY;
            playerStateMachine.CurrentMovementY = playerStateMachine.CurrentMovementY + (playerStateMachine.InitialJumpGravity * Time.deltaTime);
            playerStateMachine.AppliedMovementY = (previousYVelocity + playerStateMachine.CurrentMovementY) * .5f;
        }
    }
}