using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WallState : RootState, IState
    {
        private CharacterInfo currentCharacter;

        public WallState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;
        }

        public void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public void OnExit()
        {
            throw new System.NotImplementedException();
        }

        public void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}
