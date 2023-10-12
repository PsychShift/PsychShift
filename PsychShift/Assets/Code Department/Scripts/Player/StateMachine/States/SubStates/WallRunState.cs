using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class WallRunState : RootState, IState
    {
        private CharacterInfo currentCharacter;
        public WallRunState(PlayerStateMachine playerStateMachine)
        {
            this.playerStateMachine = playerStateMachine;
        }

        public void Tick()
        {
            SubStateTick();
        }

        public void OnEnter()
        {
            currentCharacter = playerStateMachine.currentCharacter;
        }

        public void OnExit()
        {
            
        }
    }
}
