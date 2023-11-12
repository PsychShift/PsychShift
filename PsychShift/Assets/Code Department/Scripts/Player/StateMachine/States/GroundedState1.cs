using UnityEngine;

namespace Player
{
    public class GroundedState1 : RootState1, IState
    {
        private CharacterInfo currentCharacter;
        public GroundedState1(PlayerStateMachine1 playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;
        }
        
        public void Tick()
        {
//            Debug.Log(InputManager.Instance.GetPlayerMovement());
            SubStateTick();
            HandleGravity();
        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
            currentSubState = stateMachine._currentSubState;
            SetSubState();
            //foreach(IState state in subStates) Debug.Log(state);
        }

        public void OnExit()
        {
            currentSubState.OnExit();
            stateMachine._currentSubState = currentSubState;
            playerStateMachine.InAirForward = Vector3.zero;
            playerStateMachine.InAirRight = Vector3.zero;
        }

        private void HandleGravity()
        {
            playerStateMachine.CurrentMovementY = playerStateMachine.gravityValue;
            playerStateMachine.AppliedMovementY = playerStateMachine.gravityValue;
        }

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }
    }
}