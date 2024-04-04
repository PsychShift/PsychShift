using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Guns;
[CustomEditor(typeof(ShootConfigScriptableObject))]
public class ShootConfigScriptableObject : Editor
{
    public override void OnInspectorGUI()
    {
        // Display other inspector elements here

        // Check if the button is pressed
        if (GUILayout.Button("Generate"))
        {
            Guns.ShootConfigScriptableObject selector = (Guns.ShootConfigScriptableObject)target;
            
            BulletSpreadVisualizer.UpdateTexture(selector.gun);
        }
        base.OnInspectorGUI();
    }
}
