using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using BrainSwapSaving;

[CustomEditor(typeof(ModelSaving), true)]
public class ModelSavingEditor : Editor
{
    //public SerializedDictionary<int, string> boneProp;
    public ModelSave model;
    private string fileName;
    public override void OnInspectorGUI()
    {
        // Display other inspector elements here

        // Check if the button is pressed
        if (GUILayout.Button("Test Save"))
        {
            ModelSaving script = (ModelSaving)target;
            // find the props on the enemy, this assigns the depth of the props and the transform of the prop
            // to the scriptable object, but the SO cant use this version of the transform, it needs a prefab


            model = new();
            script.fileName.ToLower();
            fileName = script.fileName;
  
            Transform root = script.transform;
            model.meshMaterials = root.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;

            CreateList(root, ref model.props);
        }

        if (GUILayout.Button("Test Load"))
        {
            ModelSaving script = (ModelSaving)target;
            // find the props on the enemy, this assigns the depth of the props and the transform of the prop
            // to the scriptable object, but the SO cant use this version of the transform, it needs a prefab
            model = new();

            string modelJSON = SaveSystem.Load(fileName);
            model = JsonUtility.FromJson<ModelSave>(modelJSON);
          }
        base.OnInspectorGUI();
    }

    public void CreateList(Transform root, ref List<PropSave> props)
    {
        int depth = 0;
        CreateListRecursive(root, ref depth, ref props);

        string json = JsonUtility.ToJson(model);

        SaveSystem.Save(json, "/" + fileName);
    }
    private void CreateListRecursive(Transform root, ref int depth, ref List<PropSave> props)
    {
        foreach(Transform child in root)
        {
            if(child.CompareTag("EnemyCosmetic"))
            {
                GameObject prefab = child.gameObject;
                Object gameObject2 = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
                string prefabPath = AssetDatabase.GetAssetPath(gameObject2);
                Object obj = AssetDatabase.LoadAssetAtPath<Object>(prefabPath);

                if (obj != null && PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.NotAPrefab)
                {
                    //boneProp.Add(depth, prefabPath);
                    props.Add(
                        new PropSave
                        (
                            depth,
                            prefabPath,
                            child.localPosition,
                            child.localEulerAngles,
                            child.localScale
                        )
                    );
                }
                else
                {
                    Debug.LogError("something whent wrong finding the prefabs");
                }
            }
            depth++;
            CreateListRecursive(child, ref depth, ref props);
        }
    }
}
[System.Serializable]
public class ModelSave
{
    public ModelSave()
    {
        props = new();
    }
    public Material[] meshMaterials;
    public List<PropSave> props;
}
[System.Serializable]
public class PropSave
{
    public PropSave(int depth, string prefabPath, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.depth = depth;
        this.prefabPath = prefabPath;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
    public int depth;
    public string prefabPath;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;
}
