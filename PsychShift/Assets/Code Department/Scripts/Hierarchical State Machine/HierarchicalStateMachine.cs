/* using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using StateMachine;
public class HierarchicalStateMachine
{
    public IState _currentState { get; private set; }
    public IState _currentSubState { get; private set; }

    private Dictionary<Type, List<Transition>> _subTransitions = new Dictionary<Type, List<Transition>>(); // Transitions for sub states
    private Dictionary<Type, List<Transition>> _rootTransitions = new Dictionary<Type, List<Transition>>(); // Transitions for root states
    private List<Transition> _currentSubTransitions = new List<Transition>(); // Switches out what transition is active for sub states
    private List<Transition> _currentRootTransitions = new List<Transition>(); // Switches out what transition is active for root states
    private List<Transition> _anySubTransitions = new List<Transition>();
    private List<Transition> _anyRootTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(capacity: 0);

    public void Tick()
    {
        var subTransition = GetSubTransition();
        if (subTransition != null)
            SetSubState(subTransition.To); // Switch sub state
            
        var transition = GetRootTransition(); // Look if a state is ready to transition
        if (transition != null) // if a transition was found
            SetRootState(transition.To); // Switch root state
        
        _currentState?.Tick(); // update function for the current state
        _currentSubState?.Tick(); // update function for the current sub state
    }

    // Handles Sub State
    // Handles Sub State
    public void SetSubState(IState state)
    {
        if (state == _currentSubState) // If the new state is the same as the last return
            return;

        _currentSubState?.OnExit(); // If we have a previous state, use the state's OnExit function
        _currentSubState = state; // Set the new state

        _subTransitions.TryGetValue(_currentSubState.GetType(), out _currentSubTransitions); // Find the list of transitions from the dictionary of a certain type and place the transitions into _currentSubTransitions
        if (_currentSubState == null) // If there wasn't a transition
            _currentSubTransitions = EmptyTransitions; // The currentSubTransitions list is empty to prevent allocation of extra memory

        _currentSubState.OnEnter(); // Call the OnEnter function for the new state
    }

    // Handles Root State
    public void SetRootState(IState state)
    {
        if (state == _currentState) // If the new state is the same as the last return
            return;
        IRootState rootState = state as IRootState;
        if(rootState == null)
            return;
        #region Handle Root State Information
        _currentState?.OnExit(); // If we have a previous state, use the state's OnExit function
        _currentState = state; // Set the new state

        _rootTransitions.TryGetValue(_currentState.GetType(), out _currentRootTransitions); // Find the list of transitions from the dictionary of a certain type and place the transitions into _currentRootTransitions
        if (_currentState == null) // If there wasn't a transition
            _currentRootTransitions = EmptyTransitions; // The currentRootTransitions list is empty to prevent allocation of extra memory
        #endregion

        /* #region Handle Sub State Information
        _currentSubState?.OnExit();

        #endregion 
        


        _currentState.OnEnter(); // Call the OnEnter function for the new state
    }

    // Add a transition for Sub State
    public void AddSubTransition(IState from, IState to, Func<bool> predicate)
    {
        if (_subTransitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _subTransitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    // Add a transition for Root State
    public void AddRootTransition(IRootState from, IRootState to, Func<bool> predicate)
    {
        if (_rootTransitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _rootTransitions[from.GetType()] = transitions;
        }

        transitions.Add(new Transition(to, predicate));
    }

    public void AddAnySubTransition(IState state, Func<bool> predicate)
    {
        _anySubTransitions.Add(item:new Transition(state, predicate));
    }
    public void AddAnyRootTransition(IState state, Func<bool> predicate)
    {
        _anyRootTransitions.Add(item:new Transition(state, predicate));
    }


    private class Transition
    {
        public Func<bool> Condition {get; }
        public IState To {get; }

        public Transition(IState to, Func<bool> condition)
        {
            To = to;
            Condition = condition;
        }
    }

    private Transition GetRootTransition()
    {
        foreach(var transition in _anyRootTransitions)
            if(transition.Condition())
                return transition;
        
        foreach(var transition in _currentRootTransitions)
            if(transition.Condition())
                return transition;
        
        return null;
    }
    private Transition GetSubTransition()
    {
        foreach(var transition in _anySubTransitions)
            if(transition.Condition())
                return transition;
        
        foreach(var transition in _currentSubTransitions)
            if(transition.Condition())
                return transition;
        
        return null;
    }
}
 */