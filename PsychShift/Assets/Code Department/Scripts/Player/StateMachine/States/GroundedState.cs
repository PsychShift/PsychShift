using UnityEngine;

namespace Player
{
    public class GroundedState : RootState, IState
    {
        private CharacterInfo currentCharacter;
        public GroundedState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;
        }
        
        public void Tick()
        {
            // Call the Tick method of the current sub-state
            SubStateTick();
            //HandleGravity();
        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
            currentSubState = stateMachine._currentSubState;
            SetSubState();
        }

        public void OnExit()
        {
            currentSubState.OnExit();
            stateMachine._currentSubState = currentSubState;
            playerStateMachine.InAirForward = Vector3.zero;
            playerStateMachine.InAirRight = Vector3.zero;
        }

        /*private void HandleGravity()
        {
            playerStateMachine.CurrentMovementY = playerStateMachine.gravityValue;
            playerStateMachine.AppliedMovementY = playerStateMachine.gravityValue;
        }*/
    }
}