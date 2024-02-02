using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using VigilPathfinding;

public class VigilAI : MonoBehaviour
{
    public float speed = 10;
    public float timeBetweenPlayerCheck = 10;

    void Awake()
    {
        vigilPathfinder = new VigilPathfinder(gridScale);
        characterController = transform.GetComponent<CharacterController>();
        SetColliders();
        LoadBoolMap();
        foreach(var collider in colliders)
        {
            collider.isTrigger = true;
        }
        colliderParent.parent = null;

        SetUpGridPathNodes();
        Vector3Int newPos = Vector3Int.FloorToInt(transform.position);
        newPos.y = 0;
        SetPath(newPos);
    }
    #region AI
    VigilPathfinder vigilPathfinder;
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
        Vector3Int currentPos = Vector3Int.FloorToInt(p);
        Vector3Int endPos = grid.ElementAt(Random.Range(0, grid.Count)).Key;

        int attempt = 0;
        while(!vigilPathfinder.FindPath(currentPos, endPos, grid, out currentPath))
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
        if (currentPathCoroutine != null)
        {
            StopCoroutine(currentPathCoroutine);
        }

        Vector3 p = transform.position;
        Vector3Int endPos = grid.ElementAt(Random.Range(0, grid.Count)).Key;

        int attempt = 0;
        while(!vigilPathfinder.FindPath(currentPos, endPos, grid, out currentPath))
        {
            endPos = grid.ElementAt(Random.Range(0, grid.Count)).Key;
            attempt++;
        }

        // Store the new coroutine
        currentPathCoroutine = MoveTo(currentPath);

        // Start the new coroutine
        StartCoroutine(currentPathCoroutine);
    }

    IEnumerator MoveTo(List<Vector3Int> waypoints)
    {
        int waypointIndex = 0;
        while (waypointIndex < waypoints.Count)
        {
            Vector3 waypoint = waypoints[waypointIndex];
            Vector3 direction = (waypoint - characterController.transform.position).normalized;
            float distanceToWaypoint = Vector3.Distance(characterController.transform.position, waypoint);
            float travelTime = distanceToWaypoint / speed;

            float elapsedTime = 0f;

            while (elapsedTime < travelTime)
            {
                characterController.Move(direction * speed * Time.deltaTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            waypointIndex++;
        }
    }
    public void SetUpGridPathNodes()
    {
        grid = new Dictionary<Vector3Int, PathNode>(boolsMap.GetLength(0) * boolsMap.GetLength(1));
        
        int i = 0;
        for(int x = 0; x < boolsMap.GetLength(0); x++)
            for(int y = 0; y < boolsMap.GetLength(1); y++)
            {
                Vector3Int gridPos = Vector3Int.FloorToInt(colliders[x, y].transform.position);
                grid[gridPos] = new PathNode(gridPos, !boolsMap[x,y]);
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
    public Vector3Int gridScale = new Vector3Int(10, 1, 10);

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

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(!enableGUIEditor) return;
        if(boolsMap == null) LoadBoolMap();
        int i = 0;
        for(int x = 0; x < boolsMap.GetLength(0); x++)
            for(int y = 0; y < boolsMap.GetLength(1); y++)
            {
                Gizmos.color = boolsMap[x,y] ? Color.green : Color.red;
                Vector3 position = colliderParent.position + new Vector3(x*gridScale.x, pos.y, y*gridScale.z);
                Gizmos.DrawCube(position, gridScale);
                Handles.Label(position, $"{i} {position}");
                i++;
            }
    }
    #endif
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

        //GameObject parent = transform.Find("Collider Parent").gameObject;
        Transform parent = transform.parent;
        GameObject colParent = parent.Find("Collider Parent").gameObject;
   
        if (colParent != null) DestroyImmediate(colParent);
        
        if(colliderParent == null)
        {
            colliderParent = new GameObject().transform;
            colliderParent.parent = parent;
            colliderParent.name = "Collider Parent";
        }
        else
        {
            DestroyImmediate(colliderParent.gameObject);
            colliderParent = new GameObject().transform;
            colliderParent.parent = parent;
            colliderParent.name = "Collider Parent";
        }
        
        for (int x = 0; x < colliders.GetLength(0); x++)
        {
            for (int y = 0; y < colliders.GetLength(1); y++)
            {
                GameObject g = new GameObject();
                g.transform.position = parent.position + new Vector3(x * gridScale.x, pos.y, y * gridScale.z);
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
