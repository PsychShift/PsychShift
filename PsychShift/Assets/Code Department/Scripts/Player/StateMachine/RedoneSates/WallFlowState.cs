using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using System;

namespace Player
{ 
    public class WallFlowState : IState
    {
        private PlayerStateMachine playerStateMachine;
        private StateMachine.StateMachine subStateMachine;
        private CharacterInfo currentCharacter;
        public WallFlowState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            void AT(IState from, IState to, Func<bool> condition) => subStateMachine.AddTransition(from, to, condition);
            void Any(IState from, Func<bool> condition) => subStateMachine.AddAnyTransition(from, condition);

            subStateMachine = new StateMachine.StateMachine();
            var idleState = new IdleState(this.playerStateMachine);
            var wallRunState = new WallRunState(this.playerStateMachine, this.playerStateMachine);

            AT(idleState, wallRunState, WallRun());
            AT(wallRunState, idleState, Stopped());

            subStateMachine.SetState(idleState);

            Func<bool> WallRun() => () => WallStateVariables.Instance.WallRight || WallStateVariables.Instance.WallLeft;
            Func<bool> Stopped() => () => InputManager.Instance.GetPlayerMovement().magnitude == 0;
        }

        public void OnEnter()
        {
            WallStateVariables.Instance.TimeOnWall = 0f;
            currentCharacter = playerStateMachine.currentCharacter;
            playerStateMachine.CurrentMovementY = 0;
            playerStateMachine.AppliedMovementY = 0;
            subStateMachine._currentState.OnEnter();
        }
        //Kevin added this call public funct from substate that swaps var that changes wallhang substate
        public void OnExit()
        {
            subStateMachine._currentState.OnExit();
        }

        public void Tick()
        {
            WallStateVariables.Instance.TimeOnWall += Time.deltaTime;
            //wallVariables.OrganizeHitsList();
            WallStateVariables.Instance.CheckWalls(playerStateMachine.currentCharacter.wallCheck);
            //Debug.Log("Forward Wall == " + WallStateVariables.Instance.ForwardWall + " Side Wall == " + WallStateVariables.Instance.SideWall);
            subStateMachine.Tick();
        }

        public Color GizmoColor()
        {
            return Color.blue;
        }
    }    
}
