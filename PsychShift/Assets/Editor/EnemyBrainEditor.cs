using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(EnemyBrain), true)]
[CanEditMultipleObjects]
public class EnemyBrainEditor : Editor
{
    private List<FieldInfo> listFields;
    void OnEnable()
    {
        EnemyBrain enemyBrain = (EnemyBrain)target;
        listFields = GetListFields(enemyBrain);
    }
    private void OnSceneGUI()
    {
        EnemyBrain enemyBrain = (EnemyBrain)target;

        foreach(FieldInfo item in listFields)
        {
            List<Vector3> points = (List<Vector3>)item.GetValue(enemyBrain);
            for (int i = 0; i < points.Count; i++)
            {
                Handles.color = Color.red;
                points[i] = Handles.PositionHandle(points[i], Quaternion.identity);
                Handles.Label(points[i], $"{i}", new GUIStyle { fontSize = 20, normal = new GUIStyleState { textColor = Color.white } });
            }
            item.SetValue(enemyBrain, points);
        }  
    }

    public class ScriptActivatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyBrain script = (EnemyBrain)target;

        EditorGUILayout.LabelField("Script Active:", script.enabled ? "Yes" : "No");
        script.enabled = EditorGUILayout.Toggle("Toggle Script", script.enabled);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }
}

    void OnValidate() {
        EnemyBrain enemyBrain = (EnemyBrain)target;
        listFields = GetListFields(enemyBrain);
        
    }

    public List<FieldInfo> GetListFields(object obj)
    {
        var type = obj.GetType();
        var listFields = new List<FieldInfo>();

        while (type != null)
        {
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                .Where(f => f.FieldType.IsGenericType && f.FieldType.GetGenericTypeDefinition() == typeof(List<>));

            listFields.AddRange(fields);

            type = type.BaseType;
        }

        return listFields;
    }
}