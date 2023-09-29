using System;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
public class RootState
{
    protected StateMachine.StateMachine stateMachine;
    protected List<IState> subStates = new();
    protected IState defaultSubState;
    protected Dictionary<Type, List<Transition>> subTransitions = new(); // Transitions for sub states
    protected List<Transition> currentSubTransitions = new();
    private static readonly List<Transition> EmptyTransitions = new List<Transition>(capacity: 0);

    protected IState currentSubState;
    public void AddSubState(IState state)
    {
        subStates.Add(state);
    }
    public void PrepareSubStates()
    {
        foreach (var state in subStates)
        {
            //Debug.Log($"{this} is referencing this states {state} transition {stateMachine.FindStateTransitions(state)}");
            SetSubStateTransitions(state, stateMachine.FindStateTransitions(state));
        }
    }
    private void SetSubStateTransitions(IState state, List<Transition> transitions)
    {
        foreach (var transition in transitions)
            if(!subStates.Contains(transition.To))
                transitions.Remove(transition);
        subTransitions.Add(state.GetType(), transitions);
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
            if(transition.Condition())
                return transition;
        
        return null;
    }

    protected void SetSubState(IState state)
    {
        if (state == currentSubState) // If the new state is the same as the last return
            return;

        currentSubState?.OnExit(); // If we have a previous state, use the state's OnExit function
        currentSubState = state; // Set the new state

        subTransitions.TryGetValue(currentSubState.GetType(), out currentSubTransitions); // Find the list of transitions from the dictionary of a certain type and place the transitions into currentSubTransitions
        if (currentSubState == null) // If there wasn't a transition
            currentSubTransitions = EmptyTransitions; // The currentSubTransitions list is empty to prevent allocation of extra memory

        currentSubState.OnEnter(); // Call the OnEnter function for the new state
    }
    protected void SetSubState()
    {
        IState state;
        if(subStates.Contains(stateMachine._currentSubState))
            state = stateMachine._currentSubState;
        else
            state = defaultSubState;
    Debug.Log(state);
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
}    


