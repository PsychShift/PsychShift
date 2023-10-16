using UnityEngine;

namespace Player
{
    public class MantleState : IState
    {
        private readonly PlayerStateMachine playerStateMachine;
        
        public MantleState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
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
