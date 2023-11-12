using UnityEngine;

namespace Player
{
    /// <summary>
    /// Root state, responsible for making an enemy Jump and handling gravity in air.
    /// </summary>
    public class JumpState : RootState, IState
    {

        public JumpState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;
        }
        
        public void Tick()
        {
            // Call the Tick method of the current sub-state
            //Debug.Log(currentSubState);
            SubStateTick();
            HandleGravity();
        }

        public void OnEnter()
        {
            currentSubState = stateMachine._currentSubState;

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
            //currentCharacter.rb.AddForce(Vector3.up * playerStateMachine.JumpForce, ForceMode.Impulse);
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

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }
    }
}