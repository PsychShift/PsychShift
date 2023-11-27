using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using System;

namespace Player
{
    public class GroundedState : IState
    {
        private PlayerStateMachine playerStateMachine;
        private StateMachine.StateMachine subStateMachine;
        private CharacterInfo currentCharacter;
        public GroundedState(PlayerStateMachine playerStateMachine)
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
            //Debug.Log(InputManager.Instance.GetPlayerMovement());
            HandleGravity();
            subStateMachine.Tick();
        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
            subStateMachine._currentState.OnEnter();
        }

        public void OnExit()
        {
            subStateMachine._currentState.OnExit();
            playerStateMachine.InAirForward = Vector3.zero;
            playerStateMachine.InAirRight = Vector3.zero;
            playerStateMachine.CurrentMovementY = -0.5f;
            playerStateMachine.AppliedMovementY = -0.5f;
        }

        private void HandleGravity()
        {
            playerStateMachine.CurrentMovementY = playerStateMachine.gravityValue;
            playerStateMachine.AppliedMovementY = playerStateMachine.gravityValue;
        }

        public Color GizmoColor()
        {
            return Color.green;
        }
    }
}