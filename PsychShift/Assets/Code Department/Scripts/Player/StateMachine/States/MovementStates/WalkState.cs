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
        Debug.Log("Hello from Walk");
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
        movement = playerStateMachine.currentCharacter.model.transform.forward * movement.z + playerStateMachine.currentCharacter.model.transform.right * movement.x;
        playerStateMachine.AppliedMovementX = movement.x * playerStateMachine.WalkSpeed;
        playerStateMachine.AppliedMovementZ = movement.z * playerStateMachine.WalkSpeed;
    }

    /* private void Move()
    {
        Vector2 input = InputManager.Instance.GetPlayerMovement();
        playerStateMachine.currentInputVector = Vector2.SmoothDamp(playerStateMachine.currentInputVector, input, ref playerStateMachine.smoothInputVelocity, playerStateMachine.smoothInputSpeed);
        playerStateMachine.playerVelocity = new Vector3(playerStateMachine.currentInputVector.x, playerStateMachine.playerVelocity.y, playerStateMachine.currentInputVector.y);
        playerStateMachine.playerVelocity = playerStateMachine.cameraTransform.forward * playerStateMachine.playerVelocity.z + playerStateMachine.cameraTransform.right * playerStateMachine.playerVelocity.x;

        currentCharacter.controller.Move(playerStateMachine.playerVelocity * Time.deltaTime * playerStateMachine.walkSpeed);
    } */
}
