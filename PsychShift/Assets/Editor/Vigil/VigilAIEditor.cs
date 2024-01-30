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
            vigilAI.UpdateBoolArray();
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

            for (int x = 0; x < vigilAI.boolsMap.GetLength(0); x++)
            {
                for (int y = 0; y < vigilAI.boolsMap.GetLength(1); y++)
                {
                    Vector3 cubeCenter = vigilAI.pos + new Vector3((x + 0.5f) * vigilAI.gridScale.x, 0, (y + 0.5f) * vigilAI.gridScale.z);
                    float sphereRadius = vigilAI.gridScale.x / 2f;

                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~0))
                    {
                        // Check if the ray intersects with the sphere
                        if ((hit.point - cubeCenter).sqrMagnitude <= sphereRadius * sphereRadius)
                        {
                            // The handle was clicked, invert the boolean value
                            vigilAI.boolsMap[x, y] = !vigilAI.boolsMap[x, y];
                            SceneView.RepaintAll();
                            break;
                        }
                    }
                }
            }
        }
    }
}
