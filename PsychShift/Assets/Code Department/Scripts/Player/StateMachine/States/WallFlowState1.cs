using UnityEngine;

namespace Player
{
    public class WallFlowState1 : RootState1, IState
    {
        private CharacterInfo currentCharacter;

        public WallFlowState1(PlayerStateMachine1 playerStateMachine, StateMachine.StateMachine stateMachine)
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
            Debug.Log("WallFlowState entered");
            WallStateVariables.Instance.TimeOnWall = 0f;
            currentCharacter = playerStateMachine.currentCharacter;
            currentSubState = stateMachine._currentSubState;
            playerStateMachine.CurrentMovementY = 0;
            playerStateMachine.AppliedMovementY = 0;
            SetSubState();
        }
        //Kevin added this call public funct from substate that swaps var that changes wallhang substate
        public void OnExit()
        {
            stateMachine._currentSubState = currentSubState;
        }

        public void Tick()
        {
            WallStateVariables.Instance.TimeOnWall += Time.deltaTime;
            SubStateTick();
            //wallVariables.OrganizeHitsList();
            WallStateVariables.Instance.CheckWalls(playerStateMachine.currentCharacter.wallCheck);
            //Debug.Log("Forward Wall == " + WallStateVariables.Instance.ForwardWall + " Side Wall == " + WallStateVariables.Instance.SideWall);
        }
    }

    
}
