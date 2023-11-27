using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using System;

namespace Player
{
    /// <summary>
    /// Root state, responsible for making an enemy Jump and handling gravity in air.
    /// </summary>
    public class JumpState : IState
    {
        private PlayerStateMachine playerStateMachine;
        private StateMachine.StateMachine subStateMachine;
        private CharacterInfo currentCharacter;
        public JumpState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            void AT(IState from, IState to, Func<bool> condition) => subStateMachine.AddTransition(from, to, condition);
            void Any(IState from, Func<bool> condition) => subStateMachine.AddAnyTransition(from, condition);

            subStateMachine = new StateMachine.StateMachine();
            var idleState = new IdleState(playerStateMachine);
            var walkState = new WalkState(playerStateMachine);

            AT(idleState, walkState, Walked());
            AT(walkState, idleState, Stopped());

            subStateMachine.SetState(idleState);

            Func<bool> Walked() => () => InputManager.Instance.GetPlayerMovement().magnitude != 0;
            Func<bool> Stopped() => () => InputManager.Instance.GetPlayerMovement().magnitude == 0;
        }
        
        public void Tick()
        {
            // Call the Tick method of the current sub-state
            //Debug.Log(currentSubState);
            HandleGravity();
            subStateMachine.Tick();
        }

        public void OnEnter()
        {
            HandleJump();
            subStateMachine._currentState.OnEnter();
        }

        public void OnExit()
        {
            subStateMachine._currentState.OnExit();
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
            return Color.white;
        }
    }
}