using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EnemyBrainSelector))]
public class EnemyBrainSelectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Display other inspector elements here

        // Check if the button is pressed
        if (GUILayout.Button("Swap Enemy Brain"))
        {
            EnemyBrainSelector selector = (EnemyBrainSelector)target;
            // Call your function when the button is pressed
            selector.SwapBrain();
        }
        base.OnInspectorGUI();
    }
}
