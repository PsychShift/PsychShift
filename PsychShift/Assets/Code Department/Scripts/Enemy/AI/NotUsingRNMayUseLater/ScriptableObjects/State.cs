using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Action", menuName = "PluggableAI/State", order = 0)]
public class State : ScriptableObject
{
    public Action[] actions;
    public Color sceneGizmoColor = Color.grey;
    public void UpdateState(StateController controller)
    {
        DoActions(controller);
    }

    private void DoActions(StateController controller)
    {
        for(int i = 0; i < actions.Length; i++)
        {
            actions[i].Act(controller);
        }
    }
}
