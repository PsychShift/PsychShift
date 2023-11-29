using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using System;
namespace Player
{
    /// <summary>
    /// Root state responsible for handling  gravity after reaching the peak of a jump or falling off a ledge.
    /// </summary> 
    public class WallFallState : IState
    {
        private PlayerStateMachine playerStateMachine;
        private StateMachine.StateMachine subStateMachine;
        private CharacterInfo currentCharacter;
        Vector3 jumpDirection;
        float jumpForce;
        float jumpAngle;
        public WallFallState(PlayerStateMachine playerStateMachine, float jumpForce, float jumpAngle)
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
            HandleGravity();
            HorizontalMovement();
            //subStateMachine.Tick();
        }

        public void OnEnter()
        {
            endTime = Time.time + 0.4f;
            currentCharacter = playerStateMachine.currentCharacter;
            //subStateMachine._currentState.OnEnter();

            jumpDirection = WallStateVariables.Instance.LastWallNormal;
            jumpDirection.Normalize();
        }

        public void OnExit()
        {
            //subStateMachine._currentState.OnExit();
        }

        private void HandleGravity()
        {
            float previousYVelocity = playerStateMachine.CurrentMovementY;
            playerStateMachine.CurrentMovementY = playerStateMachine.CurrentMovementY + playerStateMachine.gravityValue * Time.deltaTime;
            playerStateMachine.AppliedMovementY = Mathf.Max((previousYVelocity + playerStateMachine.CurrentMovementY) * .5f, -playerStateMachine.MaxFallSpeed);
            //Debug.Log(playerStateMachine.AppliedMovementY);
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
        float endTime = 0f;
        private bool DelayOver()
        {
            if (Time.time >= endTime)
            {
                Debug.Log("Delay over");
                return true;
            }
            return false;
        }
        public Func<bool> IsDone() => () => DelayOver();
        

        public Color GizmoColor()
        {
            return Color.black;
        }
    }
}