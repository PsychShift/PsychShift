using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {

        EnemySpawner myScript = (EnemySpawner)target;
        if (GUILayout.Button("Update Enemy Spawner Components"))
        {
            Undo.RecordObject(myScript, "Update Enemy Spawner Components");
            myScript.UpdateComponents();
        }
        DrawDefaultInspector();
    }
}