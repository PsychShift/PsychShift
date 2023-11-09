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

    Color GizmoColor();
}
