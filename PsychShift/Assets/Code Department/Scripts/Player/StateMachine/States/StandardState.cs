using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class StandardState : RootState, IState
    {
        private CharacterInfo currentCharacter;
        public StandardState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;
        }
        public void OnEnter()
        {
            SetSubState();
            
            InputManager.Instance.SwapControlMap(ActionMapEnum.standard);
            TimeManager.Instance.UndoSlowmotion();
        }

        public void OnExit()
        {
            
        }

        public void Tick()
        {
            Look();

            SubStateTick();
        }
    }
}