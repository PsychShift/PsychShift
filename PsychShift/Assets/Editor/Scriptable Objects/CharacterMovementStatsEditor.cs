using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CharacterMovementStatsSO))]
public class CharacterMovementStatsEditor : Editor
{
    SerializedProperty abilitiesProperty;
    string[] abilityNames = System.Enum.GetNames(typeof(AbilityType));

    private void OnEnable()
    {
        abilitiesProperty = serializedObject.FindProperty("abilities");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        if (abilitiesProperty == null)
        {
            EditorGUILayout.HelpBox("Abilities property not found!", MessageType.Error);
            return;
        }

        EditorGUI.BeginChangeCheck();

        int abilityMask = 0;
        for (int i = 0; i < abilitiesProperty.arraySize; i++)
        {
            int abilityValue = (int)abilitiesProperty.GetArrayElementAtIndex(i).enumValueIndex;
            abilityMask |= 1 << abilityValue;
        }

        abilityMask = EditorGUILayout.MaskField("Character Abilities", abilityMask, abilityNames);

        if (EditorGUI.EndChangeCheck())
        {
            for (int i = 0; i < abilitiesProperty.arraySize; i++)
            {
                int abilityValue = (int)abilitiesProperty.GetArrayElementAtIndex(i).enumValueIndex;
                if ((abilityMask & (1 << abilityValue)) == 0)
                {
                    abilitiesProperty.DeleteArrayElementAtIndex(i);
                    i--;
                }
            }

            for (int i = 0; i < abilityNames.Length; i++)
            {
                if ((abilityMask & (1 << i)) != 0)
                {
                    bool alreadyExists = false;
                    for (int j = 0; j < abilitiesProperty.arraySize; j++)
                    {
                        int abilityValue = (int)abilitiesProperty.GetArrayElementAtIndex(j).enumValueIndex;
                        if (abilityValue == i)
                        {
                            alreadyExists = true;
                            break;
                        }
                    }
                    if (!alreadyExists)
                    {
                        abilitiesProperty.InsertArrayElementAtIndex(abilitiesProperty.arraySize);
                        abilitiesProperty.GetArrayElementAtIndex(abilitiesProperty.arraySize - 1).enumValueIndex = i;
                    }
                }
            }


            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }

    }
}