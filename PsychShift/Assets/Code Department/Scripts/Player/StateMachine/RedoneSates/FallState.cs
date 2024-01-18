using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using System;
namespace Player
{
    /// <summary>
    /// Root state responsible for handling  gravity after reaching the peak of a jump or falling off a ledge.
    /// </summary> 
    public class FallState : IState
    {
        private PlayerStateMachine playerStateMachine;
        private StateMachine.StateMachine subStateMachine;
        private CharacterInfo currentCharacter;
        public FallState(PlayerStateMachine playerStateMachine)
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
        }

        private void HandleGravity()
        {
            float previousYVelocity = playerStateMachine.CurrentMovementY;
            playerStateMachine.CurrentMovementY = playerStateMachine.CurrentMovementY + playerStateMachine.gravityValue * Time.deltaTime;
            playerStateMachine.AppliedMovementY = Mathf.Max((previousYVelocity + playerStateMachine.CurrentMovementY) * .5f, -playerStateMachine.MaxFallSpeed);
            //Debug.Log(playerStateMachine.AppliedMovementY);
        }

        public Color GizmoColor()
        {
            return Color.yellow;
        }
    }
}