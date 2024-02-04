using System.Collections.Generic;
using BrainSwapSaving;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class ModelSaving : MonoBehaviour
{
    public EnemyBrainSelector selector;
    public string fileName;
    public ModelSave model;

    //private static readonly string SAVE_FOLDER = Application.dataPath + "/Design Department/CHARACTERS/ENEMY/EnemyModelPrefabs";
    private static readonly string SAVE_FOLDER = "EnemyModels";

    #region FileName
    public void RecomendedFileName()
    {
        string gunTypeName = selector.GunName(selector.gunType);
        string modifierName = selector.ModifierName(selector.modifiers);

        string recomendedFileName = gunTypeName + modifierName + "_EnemyModel";

        fileName = recomendedFileName;
        Debug.Log("The Recomended Filename is: " + fileName);
    }
    #endregion

    #if UNITY_EDITOR

    public void Save()
    {
        model = new();
        GetMaterialPaths();
        CreateList(transform, ref model.props);
    }
    public void GetMaterialPaths()
    {
        // Get the SkinnedMeshRenderer component
        SkinnedMeshRenderer skinnedMeshRenderer = transform.GetComponentInChildren<SkinnedMeshRenderer>();

        // Get the shared materials of the SkinnedMeshRenderer
        Material[] sharedMaterials = skinnedMeshRenderer.sharedMaterials;

        // Initialize an array to hold the paths to the material assets
        string[] materialPaths = new string[sharedMaterials.Length];
        string[] materialNames = new string[sharedMaterials.Length];
        // Loop through the shared materials and get the paths to the material assets
        for (int i = 0; i < sharedMaterials.Length; i++)
        {
            // Get the path to the material asset
            string materialPath = AssetDatabase.GetAssetPath(sharedMaterials[i]);

            // Store the path in the array
            string[] pathSplit = materialPath.Split('/');
            string[] newSplit = pathSplit[pathSplit.Length-1].Split('.');
            materialPaths[i] = materialPath;
            materialNames[i] = newSplit[0];
        }
        model.materialPaths = materialPaths;
        model.materialNames = materialNames;
    }
    public void CreateList(Transform root, ref List<PropSave> props)
    {
        int depth = 0;
        CreateListRecursive(root, ref depth, ref props);

        string json = JsonUtility.ToJson(model);

        SaveSystem.Save(json, SAVE_FOLDER, fileName);
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

                gameObject2 = PrefabUtility.GetCorrespondingObjectFromSource(prefab);
                prefabPath = AssetDatabase.GetAssetPath(gameObject2);
                Debug.Log(prefabPath);
                obj = AssetDatabase.LoadAssetAtPath<Object>(prefabPath);

                if (obj != null)
                {
                    if (PrefabUtility.GetPrefabInstanceStatus(obj) == PrefabInstanceStatus.NotAPrefab)
                    {
                        string[] splitPath = prefabPath.Split('/');
                        string[] newSplit = splitPath[splitPath.Length-1].Split('.');
                        PropSave save = new PropSave
                            (
                                child.parent.name,
                                prefabPath,
                                newSplit[0],
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

    #endif


    #region Loading
    public bool Load(string fileName, bool delete = true)
    {
        model = new();
        string file = SaveSystem.Load(SAVE_FOLDER, fileName);
        if(file == null)
        {
            Debug.Log(file + " " + fileName);
            return false;
        } 
        ModelSave modelSave = JsonUtility.FromJson<ModelSave>(file);
        Transform root = transform;

        if(delete)
            DeleteOldProps(root, modelSave.props.Count);
        //root.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials = modelSave.meshMaterials;

        SkinnedMeshRenderer skinnedMeshRenderer = root.GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            // Initialize an array to hold the loaded materials
            Material[] loadedMaterials = new Material[modelSave.materialPaths.Length];

            // Loop through the saved materials and load each one
            for (int i = 0; i < modelSave.materialPaths.Length; i++)
            {
                // Load the material from the Resources folder
                /* Debug.Log(modelSave.materialPaths[i]);
                Debug.Log(modelSave.materialNames[i]);

                #if UNITY_EDITOR
                    loadedMaterials[i] = (Material)AssetDatabase.LoadAssetAtPath(modelSave.materialPaths[i], typeof(Material));
                    Debug.Log("Test EnemyMaterials/" + modelSave.materialNames[i]);
                    Debug.Log(modelSave.materialPaths[i]);
                #else */
                    //loadedMaterials[i] = (Material)Resources.Load("EnemyMaterials/" + modelSave.materialNames[i], typeof(Material));
                    loadedMaterials[i] = GameAssets.Instance.GetMaterial(modelSave.materialNames[i]);
                //#endif
            }

            // Assign the loaded materials to the SkinnedMeshRenderer
            skinnedMeshRenderer.sharedMaterials = loadedMaterials;
        }

        foreach(PropSave item in modelSave.props)
        {
            Transform parent = FindChildRecursively(root, item.parent);
            GameObject model;

            //#if UNITY_EDITOR
            //    model = (GameObject)AssetDatabase.LoadAssetAtPath(item.prefabPath, typeof(GameObject));
            //#else
            //Debug.Log(item.prefabName);
            model = GameAssets.Instance.GetPrefab(item.prefabName);
            //#endif

            Transform instance = Instantiate(model, parent).transform;
            
            instance.tag = "EnemyCosmetic";
            instance.localPosition = item.position;
            instance.localEulerAngles = item.rotation;
            instance.localScale = item.scale;
        }
        return true;
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
        FindPropsRecursive(root, ref props);

        for(int i = 0; i < numOfProps; i++)
        {
            //Debug.Log(props[i]);
            #if UNITY_EDITOR
            DestroyImmediate(props[i]);
            #else
            Destroy(props[i]);
            #endif
        }
    }

    private void FindPropsRecursive(Transform root, ref GameObject[] props)
    {
        foreach(Transform child in root)
        {
            if(child.CompareTag("EnemyCosmetic"))
            {
                props[added] = child.gameObject;
                added++;
            }
            FindPropsRecursive(child, ref props);
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
    public string[] materialNames;
    public string[] materialPaths;
    public List<PropSave> props;
}

[System.Serializable]
public struct PropSave
{
    public PropSave(string parent, string prefabPath, string prefabName, Vector3 position, Vector3 rotation, Vector3 scale)
    {
        this.parent = parent;
        this.prefabPath = prefabPath;
        this.prefabName = prefabName;
        this.position = position;
        this.rotation = rotation;
        this.scale = scale;
    }
    public string parent;
    public string prefabPath;
    public string prefabName;
    public Vector3 position;
    public Vector3 rotation;
    public Vector3 scale;


    public override string ToString()
    {
        return $"Parent Name: {parent}\nPrefab Path: {prefabPath}\nPosition: {position}\nRotation: {rotation}\nScale: {scale}";
    }
}

