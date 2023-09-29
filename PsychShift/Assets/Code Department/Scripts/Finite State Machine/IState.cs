using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    /// <summary>
    /// Tick should be called in the update function. Root states should call the Tick of its current substate.
    /// </summary>
    void Tick();
    /// <summary>
    /// OnEnter is called when a state is transitioned to.
    /// </summary>
    void OnEnter();
    /// <summary>
    /// OnExit is called when leaving a state.
    /// </summary>
    void OnExit();
    /// <summary>
    /// InitializeSubState is only to be used if the State in question is a root state. Meaning it's functionality must run on top of a sub states functionality. 
    /// I.E. Being grounded or falling are root states and walking or idle are sub states.
    /// </summary> <summary>
    /// 
    /// </summary>
    void InitializeSubState();
}
