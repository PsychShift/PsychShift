using UnityEngine;
using Player;
using CharacterInfo = Player.CharacterInfo;

namespace Player
{
    public class MantleState : IState
    {
        private readonly PlayerStateMachine playerStateMachine;
        
        public MantleState(PlayerStateMachine playerStateMachine)
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
