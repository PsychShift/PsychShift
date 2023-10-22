using System;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace Player
{
    public class RootState
    {
        protected StateMachine.StateMachine stateMachine;
        protected PlayerStateMachine playerStateMachine;
        protected List<IState> subStates = new();
        protected IState defaultSubState;
        protected Dictionary<Type, List<Transition>> subTransitions = new(); // Transitions for sub states
        protected List<Transition> currentSubTransitions = new(); // Transitions for the current sub state
        private static readonly List<Transition> EmptyTransitions = new(capacity: 0);

        protected IState currentSubState;
        public void AddSubState(IState state)
        {
            subStates.Add(state);
        }
        public void PrepareSubStates()
        {
            if(subStates.Count <= 1)
                return;
            foreach (var state in subStates)
            {
                //Debug.Log($"{this} is referencing this states {state} transition {stateMachine.FindStateTransitions(state)}");
                SetSubStateTransitions(state, stateMachine.FindStateTransitions(state));
            }
        }
        private void SetSubStateTransitions(IState state, List<Transition> transitions)
        {
            //Debug.Log($"Root state {this}'s sub state {state}, has {transitions.Count} transitions before SetSubstateTransitions");
            List<Transition> transitionsToAdd = new();
            foreach (var transition in transitions)
                if(subStates.Contains(transition.To))
                    transitionsToAdd.Add(transition);

            //Debug.Log($"Root state {this}'s sub state {state}, has {transitionsToAdd.Count} transitions after SetSubstateTransitions");
            subTransitions.Add(state.GetType(), transitionsToAdd);
        }
        
        /// <summary>
        /// SubStateTick handles the tick for a substate and checks for transitions
        /// </summary>
        protected void SubStateTick()
        {
            var subTransition = GetSubTransition();
            if (subTransition != null)
                SetSubState(subTransition.To); // Switch sub state

            currentSubState.Tick();
        }

        private Transition GetSubTransition()
        {
            foreach(var transition in currentSubTransitions)
            {
                if(transition.Condition())
                    return transition;
            }
            
            return null;
        }

        protected void SetSubState(IState state)
        {
            //Debug.Log($"Set Sub State ({state})");
            if (state == currentSubState || !subTransitions.ContainsKey(state.GetType())) // If the new state is the same as the last return
                return;

            currentSubState?.OnExit(); // If we have a previous state, use the state's OnExit function
            currentSubState = state; // Set the new state

            currentSubTransitions = subTransitions[currentSubState.GetType()]; // Find the list of transitions from the dictionary of a certain type and place the transitions into currentSubTransitions
            //Debug.Log($"Current Sub Transitions Length: {currentSubTransitions.Count}");
            currentSubState.OnEnter(); // Call the OnEnter function for the new state
        }
        protected void SetSubState()
        {
            IState state;
            if(subStates.Contains(stateMachine._currentSubState))
                state = stateMachine._currentSubState;
            else
                state = defaultSubState;

            SetSubState(state);
        }

        public void SetDefaultSubState(IState state)
        {
            if(!subStates.Contains(state))
            {
                Debug.LogError($"The state {state} is not a sub state of {this}, therefore cannot be set as the default state. The default state was set to {subStates[0]} instead.");
                defaultSubState = subStates[0];
                return;
            }
            defaultSubState = state;
        }

        public virtual void Look()
        {
            Vector2 mouseDelta = InputManager.Instance.GetMouseDelta();
            Vector3 currentRotation = playerStateMachine.cameraTransform.localRotation.eulerAngles;

            currentRotation.x -= mouseDelta.y;
            currentRotation.y += mouseDelta.x;

            currentRotation.x = Mathf.Clamp(currentRotation.x, -90f, 90f);

            playerStateMachine.cameraTransform.localRotation = Quaternion.Euler(currentRotation);
            playerStateMachine.currentCharacter.model.transform.rotation = Quaternion.Euler(0f, currentRotation.y, 0f);

        }
    }    
}


