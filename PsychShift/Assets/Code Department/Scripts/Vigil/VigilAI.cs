using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VigilPathfinding;

public class VigilAI : MonoBehaviour
{

    void Awake()
    {
        //characterController = transform.AddComponent<CharacterController>();
        SetColliders();
        LoadBoolMap();
        foreach(var collider in colliders)
        {
            collider.isTrigger = true;
        }
        colliderParent.parent = null;

        SetUpGridPathNodes();
        Vector3Int newPos = grid.ElementAt(0).Key;
        transform.position = newPos;
        SetPath(newPos);
    }
    #region AI
    private Dictionary<Vector3Int, PathNode> grid;
    private List<Vector3Int> currentPath;
    CharacterController characterController;

    private IEnumerator currentPathCoroutine;


    int maxAttempts = 10;
    public void SetPath()
    {
        if (currentPathCoroutine != null)
        {
            StopCoroutine(currentPathCoroutine);
        }
        Vector3 p = transform.position;
        Vector3Int currentPos = new Vector3Int((int)p.x, (int)p.y, (int)p.z);
        Vector3Int endPos = grid.ElementAt(Random.Range(0, grid.Count)).Key;

        int attempt = 0;
        while(!VigilPathfinder.FindPath(currentPos, endPos, grid, out currentPath))
        {
            endPos = grid.ElementAt(Random.Range(0, grid.Count)).Key;
            attempt++;
            Debug.Log(attempt);
        }

        // Store the new coroutine
        currentPathCoroutine = MoveTo(currentPath);

        // Start the new coroutine
        StartCoroutine(currentPathCoroutine);
    }

    public void SetPath(Vector3Int currentPos)
    {
        Debug.Log("setpath 1");
        if (currentPathCoroutine != null)
        {
            StopCoroutine(currentPathCoroutine);
        }

        Vector3 p = transform.position;
        Vector3Int endPos = grid.ElementAt(Random.Range(0, grid.Count)).Key;

        int attempt = 0;
        while(!VigilPathfinder.FindPath(currentPos, endPos, grid, out currentPath))
        {
            endPos = grid.ElementAt(Random.Range(0, grid.Count)).Key;
            attempt++;
            Debug.Log(attempt);
        }

        // Store the new coroutine
        currentPathCoroutine = MoveTo(currentPath);

        // Start the new coroutine
        StartCoroutine(currentPathCoroutine);
    }

    private IEnumerator MoveTo(List<Vector3Int> path)
    {
        foreach(var point in path)
        {
            Vector3 targetPosition = point;
            while(Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
                yield return null;
            }

            // Check if this was the last point in the path
            if (point == path[path.Count - 1])
            {
                // Stop the previous coroutine
                if (currentPathCoroutine != null)
                {
                    StopCoroutine(currentPathCoroutine);
                }

                // Store the new coroutine
                currentPathCoroutine = MoveTo(currentPath);

                // Start the new coroutine
                StartCoroutine(currentPathCoroutine);
            }
        }
    }
    public void SetUpGridPathNodes()
    {
        grid = new Dictionary<Vector3Int, PathNode>(boolsMap.GetLength(0) * boolsMap.GetLength(1));
        
        int i = 0;
        for(int x = 0; x < boolsMap.GetLength(0); x++)
            for(int y = 0; y < boolsMap.GetLength(1); y++)
            {
                Vector3Int gridPos = new Vector3Int(x, (int)pos.y, y);
                grid[gridPos] = new PathNode(gridPos, boolsMap[x,y]);
                i++;
            }
    }
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
    [SerializeField] public bool[] boolArray
    {
        get { return _boolArray; }
        set
        {
            _boolArray = value;
            //LoadBoolMap();
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
                Gizmos.DrawCube(position, gridScale);
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
        SetColliders();
    }

    public void SaveBoolArray()
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
        SetColliders();
    }

    public void LoadBoolMap()
    {
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
        SetColliders();

    }

    public void SetColliders()
    {
        if(colliders == null) colliders = new BoxCollider[gridSize.x, gridSize.y];

        GameObject parent = transform.Find("Collider Parent").gameObject;
        if(parent != null) DestroyImmediate(parent);
        
        if(colliderParent == null)
        {
            colliderParent = new GameObject().transform;
            colliderParent.parent = transform;
            colliderParent.name = "Collider Parent";
        }
        else
        {
            DestroyImmediate(colliderParent.gameObject);
            colliderParent = new GameObject().transform;
            colliderParent.parent = transform;
            colliderParent.name = "Collider Parent";
        }
        
        for (int x = 0; x < colliders.GetLength(0); x++)
        {
            for (int y = 0; y < colliders.GetLength(1); y++)
            {
                GameObject g = new GameObject();
                g.transform.position = pos + new Vector3(x * gridScale.x, pos.y, y * gridScale.z);
                g.transform.rotation = Quaternion.identity;
                g.transform.parent = colliderParent.transform;
                g.name = "Collider: (" + x + ", " + y + ")";
                colliders[x, y] = g.AddComponent<BoxCollider>();
                colliders[x, y].size = gridScale;
                VigilTileCollider vtc = g.AddComponent<VigilTileCollider>();
                vtc.C = new Vector2Int(x, y);
                g.layer = LayerMask.NameToLayer("VigilTile");
            }
        }
    }

    void OnValidate()
    {

    }
    #endregion
}
