using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISubstate : IState
{
    void SetParentState(IState parentState);
}
