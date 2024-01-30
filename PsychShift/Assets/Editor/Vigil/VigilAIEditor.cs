using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(VigilAI))]
public class VigilAIEditor : Editor
{
    public override void OnInspectorGUI()
    {
        VigilAI vigilAI = (VigilAI)target;

        // Draw the default inspector
        DrawDefaultInspector();

        // Add a button to initialize all bools to true
        if (GUILayout.Button("Save Changes"))
        {
            vigilAI.SaveBoolArray();
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("Load Changes"))
        {
            vigilAI.LoadBoolMap();
            SceneView.RepaintAll();
        }
        if (GUILayout.Button("RESET ALL TO TRUE"))
        {
            vigilAI.SetAllBoolsMapToTrue();
            SceneView.RepaintAll();
        }
    }

    private void OnEnable()
    {
        SceneView.duringSceneGui += DuringSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= DuringSceneGUI;
    }

    private void DuringSceneGUI(SceneView sceneView)
    {
        VigilAI vigilAI = (VigilAI)target;
        if(!vigilAI.enableGUIEditor) return;
        Event current = Event.current;
        Ray ray = HandleUtility.GUIPointToWorldRay(current.mousePosition);

        if (current.type == EventType.MouseDown)
        {
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0))
            {
                Handles.color = Color.red;
                Handles.DrawLine(ray.origin, hit.point);
                // Check if the ray intersects with the sphere
                if (hit.collider.TryGetComponent(out VigilTileCollider vtc))
                {
                    Vector2Int vec = vtc.GetTileVector2();
                    // The handle was clicked, invert the boolean value
                    vigilAI.boolsMap[vec.x, vec.y] = !vigilAI.boolsMap[vec.x, vec.y];
                    SceneView.RepaintAll();
                }
            }
        }
    }
}
