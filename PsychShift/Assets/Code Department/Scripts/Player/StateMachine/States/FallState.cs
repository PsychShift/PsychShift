using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Root state responsible for handling  gravity after reaching the peak of a jump or falling off a ledge.
/// </summary> 
public class FallState : IState
{
    private readonly PlayerStateMachine playerStateMachine;
    private CharacterInfo currentCharacter;
    public FallState(PlayerStateMachine playerStateMachine)
    {
        this.playerStateMachine = playerStateMachine;
    }
    public void Tick()
    {
        Move();
        HandleGravity();
    }

    public void OnEnter()
    {
        Debug.Log("Hello from Fall");
        currentCharacter = playerStateMachine.currentCharacter;
    }

    public void OnExit()
    {
        
    }

    private void HandleGravity()
    {
        playerStateMachine.playerVelocity.y += playerStateMachine.gravityValue * Time.deltaTime;
        currentCharacter.controller.Move(playerStateMachine.playerVelocity * Time.deltaTime);
    }

    private void Move()
    {
        Vector2 input = InputManager.Instance.GetPlayerMovement();
        playerStateMachine.currentInputVector = Vector2.SmoothDamp(playerStateMachine.currentInputVector, input, ref playerStateMachine.smoothInputVelocity, playerStateMachine.smoothInputSpeed);
        Vector3 move = new Vector3(playerStateMachine.currentInputVector.x, 0f, playerStateMachine.currentInputVector.y);
        move = playerStateMachine.cameraTransform.forward * move.z + playerStateMachine.cameraTransform.right * move.x;
        move.y = 0f;
        currentCharacter.controller.Move(move * Time.deltaTime * playerStateMachine.walkSpeed);
    }

    public void InitializeSubState()
    {
        
    }
}