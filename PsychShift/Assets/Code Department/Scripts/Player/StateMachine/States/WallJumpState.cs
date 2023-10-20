using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WallJumpState : RootState, IState
    {
        private CharacterInfo currentCharacter;

        public WallJumpState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;
        }

        public void Tick()
        {
            WallStateVariables.Instance.TimeOffWall += Time.deltaTime;
            HandleGravity();
            SubStateTick();
        }

        public void OnEnter()
        {
            WallStateVariables.Instance.TimeOffWall = 0f;
            Debug.Log("entered wall jump");
            currentCharacter = playerStateMachine.currentCharacter;
            currentSubState = stateMachine._currentSubState;
            /* playerStateMachine.InAirForward = currentCharacter.model.transform.forward;
            playerStateMachine.InAirRight = currentCharacter.model.transform.right; */
            HandleJump();
            SetSubState();
        }

        public void OnExit()
        {
            Debug.Log("exited wall jump");
            stateMachine._currentSubState = currentSubState;
        }

        private void HandleJump()
        {
            Vector3 dir = WallStateVariables.Instance.LastWallNormal;

            playerStateMachine.AppliedMovementX = dir.x * playerStateMachine.InitialJumpVelocity * 5;
            playerStateMachine.AppliedMovementZ = dir.z * playerStateMachine.InitialJumpVelocity * 5;
            playerStateMachine.CurrentMovementY = playerStateMachine.InitialJumpVelocity;
            playerStateMachine.AppliedMovementY = playerStateMachine.InitialJumpVelocity;
        }

        private void HandleGravity()
        {
            bool isFalling = playerStateMachine.CurrentMovementY <= 0f || !InputManager.Instance.IsJumpPressed;
            float fallMultiplier = 3.0f;

            if(isFalling)
            {
                Debug.Log("From Jump - Is Falling == true");
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
    }
}
