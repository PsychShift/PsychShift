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
            SubStateTick();
        }

        public void OnEnter()
        {
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
            Vector3 dir = Vector3.Cross(Vector3.up, WallStateVariables.Instance.LastWallNormal);
            
        }
    }
}
