using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StateMachine
{
    public IState _currentState { get; private set; }
    
    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>(); // Saves all transitions
    private List<Transition> _currentTransitions = new List<Transition>(); // Switches out what transition is active
    private List<Transition> _anyTransitions = new List<Transition>();

    private static List<Transition> EmptyTransitions = new List<Transition>(capacity: 0);

    public void Tick()
    {
        var transition = GetTransition(); // Look if a state is ready to transition
        if (transition != null) // if a transition was found
            SetState(transition.To); // Switch states
        
        _currentState?.Tick(); // update function for the current state
    }

    public void SetState(IState state)
    {
        if(state == _currentState) // If the new state is the same as the last return
            return;
        
        _currentState?.OnExit(); // If we have a previous state, use the states on exit function
        _currentState = state; // Set the new state

        _transitions.TryGetValue(_currentState.GetType(), out _currentTransitions); // Find the list of transitions from the dictionary of a certain type place the transition into _currentTransitions
        if (_currentState == null) // If there wasn't a transtion
            _currentTransitions = EmptyTransitions; // the currentTransitions list is empty / prevents allocation of extra memory
        

        _currentState.OnEnter(); // Call the OnEnter function for the new state
    }

    public void AddTransition(IState from, IState to, Func<bool> predicate)
    {
        if (_transitions.TryGetValue(from.GetType(), out var transitions) == false)
        {
            transitions = new List<Transition>();
            _transitions[from.GetType()] = transitions;
        }

        transitions.Add(item: new Transition(to, predicate));
    }

    public void AddAnyTransition(IState state, Func<bool> predicate)
    {
        _anyTransitions.Add(item:new Transition(state, predicate));
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

    private Transition GetTransition()
    {
        foreach(var transition in _anyTransitions)
            if(transition.Condition())
                return transition;
        
        foreach(var transition in _currentTransitions)
            if(transition.Condition())
                return transition;
        
        return null;
    }
}
