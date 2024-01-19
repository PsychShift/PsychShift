using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;

namespace Player
{
    public class IdleState : IState
    {
        private readonly PlayerStateMachine playerStateMachine;
        private CharacterInfo currentCharacter;
        public IdleState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }

        public void Tick()
        {
            Move();
        }

        private void Move()
        {
            Vector2 input = InputManager.Instance.GetPlayerMovement();
            playerStateMachine.currentInputVector = Vector2.SmoothDamp(playerStateMachine.currentInputVector, input, ref playerStateMachine.smoothInputVelocity, playerStateMachine.smoothInputSpeed);
            Vector3 movement = new Vector3(playerStateMachine.currentInputVector.x, 0f, playerStateMachine.currentInputVector.y);
            // get the rotation of the camera, isolate the y axis, and rotate the movement vector by that amount
            movement = Quaternion.Euler(0f, playerStateMachine.cameraTransform.eulerAngles.y, 0f) * movement;
            //movement = playerStateMachine.currentCharacter.characterContainer.transform.forward * movement.z + playerStateMachine.currentCharacter.characterContainer.transform.right * movement.x;
            playerStateMachine.AppliedMovementX = movement.x * playerStateMachine.WalkSpeed;
            playerStateMachine.AppliedMovementZ = movement.z * playerStateMachine.WalkSpeed;

        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
        }

        public void OnExit()
        {

        }

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }
    }
}
