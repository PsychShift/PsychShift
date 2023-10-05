using UnityEngine;

namespace Player
{
    public class SlowState : RootState, IState
    {
        private CharacterInfo currentCharacter;
        private Outliner currentOutlinedObject;
        public SlowState(PlayerStateMachine playerStateMachine, StateMachine.StateMachine stateMachine)
        {
            this.playerStateMachine = playerStateMachine;
            this.stateMachine = stateMachine;
        }
        public void OnEnter()
        {
            SetSubState();
            InputManager.Instance.SwapControlMap(ActionMapEnum.slow);
            TimeManager.Instance.DoSlowmotion(0.1f);
        }

        public void OnExit()
        {
            if(currentOutlinedObject != null)
            {
                currentOutlinedObject.ActivateOutline(false);
                currentOutlinedObject = null;
            }
        }

        public void Tick()
        {
            Look();
            SearchForInteractable();
            SubStateTick();
        }

        private void SearchForInteractable()
        {
            GameObject hitObject = playerStateMachine.CheckForCharacter();
            // Check if the hit GameObject has the specified script component
            if(hitObject == null) return;
            Outliner outliner = hitObject.GetComponent<Outliner>();
            if(outliner == null)
            {
                if(currentOutlinedObject != null)
                    currentOutlinedObject.ActivateOutline(false);
                return;
            }
            currentOutlinedObject = outliner;
            currentOutlinedObject.ActivateOutline(true);

        }
    }
}

