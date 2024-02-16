using Guns.Demo;
using Unity.VisualScripting;
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
            EnemyGunSelector gunSelector = target.GetComponent<EnemyGunSelector>();
            // Call your function when the button is pressed
            selector.SwapBrain();
            EditorUtility.SetDirty(target);
            EditorUtility.SetDirty(gunSelector);
        }
        base.OnInspectorGUI();
    }
}
