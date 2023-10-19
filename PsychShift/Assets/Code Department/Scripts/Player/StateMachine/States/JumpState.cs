using UnityEngine;

namespace Player
{
    /// <summary>
    /// Root state, responsible for making an enemy Jump and handling gravity in air.
    /// </summary>
    public class JumpState : RootState, IState
    {
        private CharacterInfo currentCharacter;
        private float timer;
        public JumpState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;
        }
        
        public void Tick()
        {
            // Call the Tick method of the current sub-state
            SubStateTick();
            HandleGravity();
        }

        public void OnEnter()
        {
            timer = 0;
            Debug.Log("Enter Jump");
            currentCharacter = playerStateMachine.currentCharacter;
            currentSubState = stateMachine._currentSubState;

            HandleJump();
            SetSubState();
        }

        public void OnExit()
        {
            Debug.Log("Exit Jump");
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
                Debug.Log("hi?");
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
}