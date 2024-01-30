using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VigilPathfinding;

public class VigilAI : MonoBehaviour
{

    #region AI
    
    #endregion

    #region Grid
    [Header("Grid Section")]
    public bool enableGUIEditor;
    public Vector3 pos;
    [Tooltip("The x and y are how many tiles there are.")]
    public Vector2Int gridSize = Vector2Int.one;
    [Tooltip("The size of each tile")]
    public Vector3 gridScale = new Vector3(10, 1, 10);

    [SerializeField] private bool[] _boolArray;
    private Transform colliderParent;
    public BoxCollider[,] colliders;
    public bool[] boolArray
    {
        get { return _boolArray; }
        set
        {
            _boolArray = value;
            LoadBoolMap();
            #if UNITY_EDITOR
            SceneView.RepaintAll();
            #endif
        }
    }
    public bool[,] boolsMap;


    void OnDrawGizmos()
    {
        if(!enableGUIEditor) return;
        if(boolsMap == null) LoadBoolMap();
        int i = 0;
        for(int x = 0; x < boolsMap.GetLength(0); x++)
            for(int y = 0; y < boolsMap.GetLength(1); y++)
            {
                Gizmos.color = boolsMap[x,y] ? Color.green : Color.red;
                Vector3 position = pos + new Vector3(x*gridScale.x, pos.y, y*gridScale.z);
                Gizmos.DrawWireCube(position, gridScale);
                Handles.Label(position, $"{i}");
                i++;
            }
    }

    public void SetAllBoolsMapToTrue()
    {
        boolsMap = new bool[gridSize.x, gridSize.y];
        for (int x = 0; x < boolsMap.GetLength(0); x++)
        {
            for (int y = 0; y < boolsMap.GetLength(1); y++)
            {
                boolsMap[x, y] = true;
            }
        }
    }

    public void UpdateBoolArray()
    {
        int i = 0;
        boolArray = new bool[gridSize.x * gridSize.y];
        for (int x = 0; x < boolsMap.GetLength(0); x++)
        {
            for (int y = 0; y < boolsMap.GetLength(1); y++)
            {
                boolArray[i] = boolsMap[x, y];
                i++;
            }
        }
    }

    public void LoadBoolMap()
    {
        SetColliders();
        boolsMap = new bool[gridSize.x, gridSize.y];
        int i = 0;
        for (int x = 0; x < boolsMap.GetLength(0); x++)
        {
            for (int y = 0; y < boolsMap.GetLength(1); y++)
            {
                boolsMap[x, y] = boolArray[i];
                i++;
            }
        }

    }

    public void SetColliders()
    {
        if(colliders == null) colliders = new BoxCollider[gridSize.x, gridSize.y];
        
        if(colliderParent == null)
        {
            colliderParent = Instantiate(new GameObject(), transform).transform;
            colliderParent.name = "Collider Parent";
        } 
        
        for (int x = 0; x < colliders.GetLength(0); x++)
        {
            for (int y = 0; y < colliders.GetLength(1); y++)
            {
                GameObject g = Instantiate(new GameObject(), pos + new Vector3(x * gridScale.x, pos.y, y * gridScale.y), Quaternion.identity, colliderParent);
                g.name = "Collider: (" + x + ", " + y + ")";
                colliders[x, y] = g.AddComponent<BoxCollider>();
                colliders[x, y].size = gridScale;
            }
        }
    }

    void OnValidate()
    {

    }
    #endregion
}
