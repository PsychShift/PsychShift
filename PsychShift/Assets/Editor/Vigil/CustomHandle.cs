using UnityEditor;
using UnityEngine;

/* public class CustomHandle
{
    public static bool ClickHandle(Vector3 position, float size, Handles.DrawCapFunction capFunc, Color color, out Vector3 newPosition)
    {
        int id = GUIUtility.GetControlID(FocusType.Passive);
        Event evt = Event.current;
        Vector3 screenPos = HandleUtility.WorldToGUIPoint(position);
        bool wasUsed = false;

        switch (evt.GetTypeForControl(id))
        {
            case EventType.MouseDown:
                if (GUIUtility.hotControl != id && HandleUtility.nearestControl == id)
                {
                    GUIUtility.hotControl = id;
                    evt.Use();
                }
                break;

            case EventType.MouseUp:
                if (GUIUtility.hotControl == id)
                {
                    GUIUtility.hotControl = 0;
                    evt.Use();
                    wasUsed = true;
                }
                break;

            case EventType.MouseDrag:
                if (GUIUtility.hotControl == id)
                {
                    evt.Use();
                }
                break;

            case EventType.Repaint:
                Handles.color = color;
                capFunc(id, screenPos, Quaternion.identity, size);
                break;
        }

        newPosition = Handles.matrix.inverse.MultiplyPoint(HandleUtility.GUIPointToWorldRay(screenPos).origin);
        return wasUsed;
    }
} */