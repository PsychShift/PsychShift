using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WallRunState : RootState, IState
    {
        private CharacterInfo currentCharacter;
        public WallRunState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
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
            currentCharacter = playerStateMachine.currentCharacter;
            SetSubState();
        }

        public void OnExit()
        {
            stateMachine._currentSubState = currentSubState;
            
        }

        private void HandleGravity()
        {
            playerStateMachine.CurrentMovementY = playerStateMachine.gravityValue;
            playerStateMachine.AppliedMovementY = playerStateMachine.gravityValue;
        }
    }
}
