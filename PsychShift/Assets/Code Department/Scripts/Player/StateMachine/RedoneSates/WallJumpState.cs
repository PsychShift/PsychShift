using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using System;

namespace Player
{
    public class WallJumpState : IState
    {
        private PlayerStateMachine playerStateMachine;
        private StateMachine.StateMachine subStateMachine;
        private CharacterInfo currentCharacter;
        float jumpForce;
        Vector3 jumpDirection;
        float jumpAngle;
        public WallJumpState(PlayerStateMachine playerStateMachine, float jumpForce, float jumpAngle)
        {
            this.playerStateMachine = playerStateMachine;
            this.jumpForce = jumpForce;
            this.jumpAngle = jumpAngle;

            /* void AT(IState from, IState to, Func<bool> condition) => subStateMachine.AddTransition(from, to, condition);
            void Any(IState from, Func<bool> condition) => subStateMachine.AddAnyTransition(from, condition);

            subStateMachine = new StateMachine.StateMachine();
            var idleState = new IdleState(playerStateMachine);
            var walkState = new WalkState(playerStateMachine);

            AT(idleState, walkState, Walked());
            AT(walkState, idleState, Stopped());

            subStateMachine.SetState(idleState);

            Func<bool> Walked() => () => InputManager.Instance.GetPlayerMovement().magnitude != 0;
            Func<bool> Stopped() => () => InputManager.Instance.GetPlayerMovement().magnitude == 0; */
        }

        public void Tick()
        {
            WallStateVariables.Instance.TimeOffWall += Time.deltaTime;
            HandleGravity();
            HorizontalMovement();
            //subStateMachine.Tick();
        }

        public void OnEnter()
        {
            WallStateVariables.Instance.TimeOffWall = 0f;
            currentCharacter = playerStateMachine.currentCharacter;
            /* playerStateMachine.InAirForward = currentCharacter.model.transform.forward;
            playerStateMachine.InAirRight = currentCharacter.model.transform.right; */
            HandleJump();
            //subStateMachine._currentState.OnEnter();
        }

        public void OnExit()
        {
            //subStateMachine._currentState.OnExit();
        }


        private void HandleJump()
        {
            jumpDirection = WallStateVariables.Instance.LastWallNormal;
            jumpDirection.Normalize();
            playerStateMachine.AppliedMovementX = jumpDirection.x * jumpForce;
            playerStateMachine.AppliedMovementZ = jumpDirection.z * jumpForce;
            playerStateMachine.CurrentMovementY = playerStateMachine.InitialJumpVelocity;
            playerStateMachine.AppliedMovementY = playerStateMachine.InitialJumpVelocity;
        }

        private void HandleGravity()
        {
            bool isFalling = playerStateMachine.CurrentMovementY <= 0f || !InputManager.Instance.IsJumpPressed;
            float fallMultiplier = 3.0f;

            if(isFalling)
            {
                float previousYVelocity = playerStateMachine.CurrentMovementY;
                playerStateMachine.CurrentMovementY = playerStateMachine.CurrentMovementY + (playerStateMachine.InitialJumpGravity * fallMultiplier * Time.deltaTime);
                playerStateMachine.AppliedMovementY = Mathf.Max((previousYVelocity + playerStateMachine.CurrentMovementY) * .5f, -playerStateMachine.MaxFallSpeed);
            }
            else
            {
                float previousYVelocity = playerStateMachine.CurrentMovementY;
                playerStateMachine.CurrentMovementY = playerStateMachine.CurrentMovementY + (playerStateMachine.InitialJumpGravity * Time.deltaTime);
                playerStateMachine.AppliedMovementY = (previousYVelocity + playerStateMachine.CurrentMovementY) * .5f;
            }
        }

        private void HorizontalMovement()
        {
            Vector2 input = InputManager.Instance.GetPlayerMovement();
            playerStateMachine.currentInputVector = Vector2.SmoothDamp(playerStateMachine.currentInputVector, input, ref playerStateMachine.smoothInputVelocity, playerStateMachine.smoothInputSpeed);
            Vector3 movement = new Vector3(playerStateMachine.currentInputVector.x, 0f, playerStateMachine.currentInputVector.y);
            // get the rotation of the camera, isolate the y axis, and rotate the movement vector by that amount
            movement = Quaternion.Euler(0f, playerStateMachine.cameraTransform.eulerAngles.y, 0f) * movement;
            //movement = playerStateMachine.currentCharacter.characterContainer.transform.forward * movement.z + playerStateMachine.currentCharacter.characterContainer.transform.right * movement.x;
            movement.Normalize();
            movement = movement != Vector3.zero ? (Vector3.Lerp(movement, jumpDirection, jumpAngle) * jumpForce) : (jumpDirection * jumpForce);
            playerStateMachine.AppliedMovementX = movement.x;
            playerStateMachine.AppliedMovementZ = movement.z;

        }

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }
    }
}
