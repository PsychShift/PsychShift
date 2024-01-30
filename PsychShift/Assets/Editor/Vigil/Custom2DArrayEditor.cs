/*using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ArrayLayout))]
public class Custom2DArrayEditor : PropertyDrawer
{
     int y = 1;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.PrefixLabel(position,label);
        Rect newPosition = position;
        newPosition.y += 18f;

        SerializedProperty data = property.FindPropertyRelative("rows");
        
        SerializedProperty xProp = property.FindPropertyRelative("xAxis");
        SerializedProperty yProp = property.FindPropertyRelative("yAxis");
        int x = xProp.intValue;
        int y = yProp.intValue;
        Debug.Log(x);
        for(int j = 0; j < y; j++)
        {
            SerializedProperty row = data.GetArrayElementAtIndex(j).FindPropertyRelative("row");
            newPosition.height = 18f;
            if(row.arraySize != x)
                row.arraySize = x;
            
            newPosition.width = position.width / x;
            for(int i = 0; i < x; i++)
            {
                bool value = row.GetArrayElementAtIndex(i).boolValue;
                value = EditorGUI.Toggle(newPosition, value);
                row.GetArrayElementAtIndex(i).boolValue = value;
                newPosition.x += newPosition.width;
            }
            newPosition.x = position.x;
            newPosition.y += 18f;
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 18f * y;
    } 
}*/