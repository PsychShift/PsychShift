using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Player
{
    public class WallStaticState1 : RootState1, IState
    {
        private CharacterInfo currentCharacter;

        public WallStaticState1(PlayerStateMachine1 playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;

        }

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }

        public void OnEnter()
        {
             //if(StaticBar.instance.currentStatic >= 1)
            //{
            WallStateVariables.Instance.TimeOnWall = 0f;
            currentCharacter = playerStateMachine.currentCharacter;
            currentSubState = stateMachine._currentSubState;
            playerStateMachine.CurrentMovementY = 0;
            playerStateMachine.AppliedMovementY = 0;
            SetSubState();
           // }
        }
        //Kevin added this call public funct from substate that swaps var that changes wallhang substate
        public void OnExit()
        {
            stateMachine._currentSubState = currentSubState;
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
            SubStateTick();
            //wallVariables.OrganizeHitsList();
            WallStateVariables.Instance.CheckWalls(playerStateMachine.currentCharacter.wallCheck);
            //Debug.Log("Forward Wall == " + WallStateVariables.Instance.ForwardWall + " Side Wall == " + WallStateVariables.Instance.SideWall);
        }
    }
}
