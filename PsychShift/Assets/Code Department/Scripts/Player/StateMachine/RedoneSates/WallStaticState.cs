using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;
using System;

namespace Player
{
    public class WallStaticState : IState
    {
        private PlayerStateMachine playerStateMachine;
        private StateMachine.StateMachine subStateMachine;
        private CharacterInfo currentCharacter;
        public WallStaticState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            void AT(IState from, IState to, Func<bool> condition) => subStateMachine.AddTransition(from, to, condition);
            void Any(IState from, Func<bool> condition) => subStateMachine.AddAnyTransition(from, condition);

            subStateMachine = new StateMachine.StateMachine();
            var wallHangState = new WallHangState(playerStateMachine);
            var vaultState = new VaultState(playerStateMachine);



            AT(wallHangState, vaultState, ClimbUpLedge());

            Func<bool> ClimbUpLedge() => () => WallStateVariables.Instance.ForwardWall && InputManager.Instance.IsJumpPressed && playerStateMachine.StaticMode;

            subStateMachine.SetState(wallHangState);
        }

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
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
            //Call public funct here when the button is pressed//Kevin Added this
            /*if(InputManager.Instance.PlayerSwitchedModeThisFrame())
            {
                //Call funct here
                WallHangState.canHangChange();
            }*/
            WallStateVariables.Instance.TimeOnWall += Time.deltaTime;
            WallStateVariables.Instance.CheckWalls(playerStateMachine.currentCharacter.wallCheck);
            subStateMachine.Tick();
        }
    }
}
