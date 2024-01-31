using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GodModeScript), true)]
public class GodModeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("God Mode Toggle"))
        {
            GodModeScript script = (GodModeScript)target;

            script.SwitchMode();
        }
        
        base.OnInspectorGUI();
    }
}
