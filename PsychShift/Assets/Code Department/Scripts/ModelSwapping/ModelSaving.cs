using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BrainSwapSaving;
using UnityEditor;
using UnityEngine;

public class ModelSaving : MonoBehaviour
{
    public string fileName;
    public ModelSave model;


    #region Saving
    public void Save()
    {
        model = new();
        model.meshMaterials = transform.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials;

        CreateList(transform, ref model.props);
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
                Object gameObject2;
                string prefabPath;
                Object obj;

                #if UNITY_EDITOR
                    gameObject2 = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
                    prefabPath = AssetDatabase.GetAssetPath(gameObject2);
                    obj = AssetDatabase.LoadAssetAtPath<Object>(prefabPath);
                #else
                    gameObject2 = Resources.Load(prefab.name);
                    prefabPath = "path/to/your/resources/folder/" + prefab.name;
                    obj = Resources.Load(prefabPath);
                #endif

                if (obj != null)
                {
                    #if UNITY_EDITOR
                        if (PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.NotAPrefab)
                    #endif
                    {
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
                        Debug.LogError("something went wrong finding the prefabs");
                    }
                }
            }
            depth++;
            CreateListRecursive(child, ref depth, ref props);
        }
    }

    #endregion

    #region Loading
    public void Load()
    {
        model = new();
        string file = SaveSystem.Load("/" + fileName);
        ModelSave modelSave = JsonUtility.FromJson<ModelSave>(file);
        Transform root = transform;

        DeleteOldProps(root, modelSave.props.Count);
        root.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = modelSave.meshMaterials;

        foreach(PropSave item in modelSave.props)
        {
            Transform parent = FindChildRecursively(root, item.parent);
            GameObject model;

            #if UNITY_EDITOR
                model = (GameObject)AssetDatabase.LoadAssetAtPath(item.prefabPath, typeof(GameObject));
            #else
                model = (GameObject)Resources.Load("path/to/your/resources/folder/" + item.prefabPath);
            #endif

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
            Debug.Log(props[i]);
            #if UNITY_EDITOR
            DestroyImmediate(props[i]);
            #else
            Destroy(props[i]);
            #endif
        }
    }

    private void DeletePropsRecursive(Transform root, ref GameObject[] props)
    {
        foreach(Transform child in root)
        {
            if(child.CompareTag("EnemyCosmetic") && !props.Contains(child.gameObject))
            {
                props[added] = child.gameObject;
                added++;
                return;
            }
            DeletePropsRecursive(child, ref props);
        }
    }
    #endregion
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
}
