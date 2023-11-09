using UnityEngine;

namespace Player
{
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
            movement = playerStateMachine.currentCharacter.characterContainer.transform.forward * movement.z + playerStateMachine.currentCharacter.characterContainer.transform.right * movement.x;
            playerStateMachine.AppliedMovementX = movement.x * playerStateMachine.WalkSpeed;
            playerStateMachine.AppliedMovementZ = movement.z * playerStateMachine.WalkSpeed;
        }

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }
        /* 
Trying to smooth out movement
private void Move()
{
   Vector2 input = InputManager.Instance.GetPlayerMovement();
   Vector3 movement = new Vector3(playerStateMachine.currentInputVector.x, 0f, playerStateMachine.currentInputVector.y);
   playerStateMachine.currentInputVector = Vector2.SmoothDamp(playerStateMachine.currentInputVector, input, ref playerStateMachine.smoothInputVelocity, playerStateMachine.smoothInputSpeed);

   float previousVelocityX = playerStateMachine.CurrentMovementX;
   float previousVelocityZ = playerStateMachine.CurrentMovementZ;
   playerStateMachine.CurrentMovementX += movement.x * playerStateMachine.WalkSpeed * Time.deltaTime;
   playerStateMachine.CurrentMovementZ += movement.z * playerStateMachine.WalkSpeed * Time.deltaTime;


   movement = playerStateMachine.currentCharacter.model.transform.forward * movement.z + playerStateMachine.currentCharacter.model.transform.right * movement.x;
   playerStateMachine.AppliedMovementX = Mathf.Min((previousVelocityX + playerStateMachine.CurrentMovementX) * 0.5f, 40f);
   playerStateMachine.AppliedMovementZ = Mathf.Min((previousVelocityZ + playerStateMachine.CurrentMovementZ) * 0.5f, 40f);
} */

        /* private Vector3 CalculateForwardRight(Vector3 move)
        {
            Vector3 forward;
            Vector3 right;
            if (playerStateMachine.currentCharacter.controller.isGrounded)
            {
                forward = playerStateMachine.currentCharacter.model.transform.forward;
                right = playerStateMachine.currentCharacter.model.transform.right;
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
}
