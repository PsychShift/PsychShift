using UnityEngine;

namespace Player
{
    public class MantleState1 : IState
    {
        private readonly PlayerStateMachine1 playerStateMachine;
        
        public MantleState1(PlayerStateMachine1 playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }

        public Color GizmoColor()
        {
            throw new System.NotImplementedException();
        }

        public void OnEnter()
        {
            Debug.Log("MantleState");
        }

        public void OnExit()
        {
            
        }

        public void Tick()
        {
            
        }
    }
}
