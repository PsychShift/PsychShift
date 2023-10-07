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
            if (hitObject == null)
            {
                // Player is not looking at any object, deactivate current outline (if any)
                if (currentOutlinedObject != null)
                {
                    currentOutlinedObject.ActivateOutline(false);
                    currentOutlinedObject = null;
                }
                return;
            }
            
            Outliner outliner = hitObject.GetComponent<Outliner>();
            
            if (outliner == null)
            {
                // Player is looking at an object without an Outliner component, deactivate current outline (if any)
                if (currentOutlinedObject != null)
                {
                    currentOutlinedObject.ActivateOutline(false);
                    currentOutlinedObject = null;
                }
                return;
            }
            
            // Player is looking at an object with an Outliner component
            if (currentOutlinedObject != null && currentOutlinedObject != outliner)
            {
                // Deactivate the outline of the previously outlined object
                currentOutlinedObject.ActivateOutline(false);
            }
            
            // Activate the outline of the currently looked at object
            currentOutlinedObject = outliner;
            currentOutlinedObject.ActivateOutline(true);
        }
    }
}

