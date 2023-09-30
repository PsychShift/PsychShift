using UnityEngine;

public class WalkState : IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public WalkState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }
    
    public void Tick()
    {
        Move();
    }

    public void OnEnter()
    {
        //Debug.Log("Hello from Walk");
        currentCharacter = playerStateMachine.currentCharacter;
    }

    public void OnExit()
    {
        
    }

    private void Move()
    {
        Vector2 input = InputManager.Instance.GetPlayerMovement();
        playerStateMachine.currentInputVector = Vector2.SmoothDamp(playerStateMachine.currentInputVector, input, ref playerStateMachine.smoothInputVelocity, playerStateMachine.smoothInputSpeed);
        Vector3 movement = new Vector3(playerStateMachine.currentInputVector.x, 0f, playerStateMachine.currentInputVector.y);
        movement = currentCharacter.model.transform.forward * movement.z + currentCharacter.model.transform.right * movement.x;
        playerStateMachine.AppliedMovementX = movement.x * playerStateMachine.WalkSpeed;
        playerStateMachine.AppliedMovementZ = movement.z * playerStateMachine.WalkSpeed;
    }
    
    /* private Vector3 CalculateForwardRight(Vector3 move)
    {
        Vector3 forward;
        Vector3 right;
        if (currentCharacter.controller.isGrounded)
        {
            forward = currentCharacter.model.transform.forward;
            right = currentCharacter.model.transform.right;
        }
        else
        {
            forward = playerStateMachine.InAirForward;
            right = playerStateMachine.InAirRight;
        }
        move = move.x * right + move.z * forward;
        
        return move;
    } */
}
