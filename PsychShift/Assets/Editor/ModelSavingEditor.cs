using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using BrainSwapSaving;
using Unity.Mathematics;

[CustomEditor(typeof(ModelSaving), true)]
public class ModelSavingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Display other inspector elements here

        // Check if the button is pressed
        if (GUILayout.Button("Test Save"))
        {
            ModelSaving script = (ModelSaving)target;

            script.Save();
        }

        if (GUILayout.Button("Test Load"))
        {
            ModelSaving script = (ModelSaving)target;
            
            script.Load();
          }
        base.OnInspectorGUI();
    }
}
    /* //public SerializedDictionary<int, string> boneProp;
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

            fileName = script.fileName;
            string modelJSON = SaveSystem.Load("/" + fileName);

            Load(script, modelJSON);
            //model = JsonUtility.FromJson<ModelSave>(modelJSON);
          }
        base.OnInspectorGUI();
    }

    #region Saving
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
                    PropSave save = new PropSave
                        (
                            child.parent.name,
                            prefabPath,
                            child.localPosition,
                            child.localEulerAngles,
                            child.localScale
                        );
                    props.Add(save);
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
    #endregion

    #region Loading
    private void Load(ModelSaving script, string file)
    {
        ModelSave modelSave = JsonUtility.FromJson<ModelSave>(file);
        // Now that we have the model info, we can modify the materials
        Transform root = script.transform;

        DeleteOldProps(root, modelSave.props.Count);
        root.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = modelSave.meshMaterials;

        foreach(PropSave item in modelSave.props)
        {

            Transform parent = FindChildRecursively(root, item.parent);
            GameObject model = (GameObject)AssetDatabase.LoadAssetAtPath(item.prefabPath, typeof(GameObject));
            Transform instance = Instantiate(model, parent).transform;
            
            instance.tag = "EnemyCosmetic";
            instance.localPosition = item.position;
            instance.localEulerAngles = item.rotation;
            instance.localScale = item.scale;
        }

    }

    private Transform FindChildRecursively(Transform root, string childName)
    {
        // Check if the current transform is the one we're looking for
        if (root.name == childName)
        {
            return root;
        }

        // Iterate over all the children of the current transform
        foreach (Transform child in root)
        {
            // Recurse on the child
            Transform found = FindChildRecursively(child, childName);
            
            // If the child is the one we're looking for, return it
            if (found != null)
            {
                return found;
            }
        }

        // If none of the children were the one we're looking for, return null
        return null;
    }

    int added = 0;
    private void DeleteOldProps(Transform root, int numOfProps)
    {
        added = 0;
        GameObject[] props = new GameObject[numOfProps];
        DeletePropsRecursive(root, ref props);

        for(int i = 0; i < numOfProps; i++)
        {
            DestroyImmediate(props[i]);
        }
    }

    private void DeletePropsRecursive(Transform root, ref GameObject[] props)
    {
        foreach(Transform child in root)
        {
            if(child.CompareTag("EnemyCosmetic"))
            {
                props[added] = child.gameObject;
                added++;
                return;
            }
            DeletePropsRecursive(child, ref props);
        }
    }
    #endregion
} */
/* [System.Serializable]
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
    public PropSave(string parent, string prefabPath, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.parent = parent;
        this.prefabPath = prefabPath;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
    public string parent;
    public string prefabPath;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;


    public override string ToString()
    {
        return $"Parent Name: {parent}\nPrefab Path: {prefabPath}\nPosition: {position}\nRotation: {rotation}\nScale: {scale}";
    }
} */
