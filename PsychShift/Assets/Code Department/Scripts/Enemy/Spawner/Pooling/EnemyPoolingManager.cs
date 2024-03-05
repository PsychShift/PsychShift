using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Guns;
using UnityEditor.SearchService;
using UnityEngine;

public class EnemyPoolingManager : MonoBehaviour
{
    [SerializeField] private AIAgression DefaultAgression;
    private static GameObject EnemyPrefab;
    public static List<PooledObjectInfo> ObjectPools = new List<PooledObjectInfo>();

    private static GameAssets Assets;
    private void Awake() {
        Assets = GameAssets.Instance;
        EnemyPrefab = Assets.EnemyPrefab;
    }


    public static EnemyBrain SpawnObject(GunType GunType, EBrainType BrainType, EEnemyModifier Modifier, AIAgression aIAgression, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.GunType == GunType && p.BrainType == BrainType && p.Modifier == Modifier);

        if(pool == null)
        {
            pool = new PooledObjectInfo() { GunType = GunType, BrainType = BrainType, Modifier = Modifier };
            ObjectPools.Add(pool);
        }

        GameObject spawnableObj = pool.InactiveObjects.FirstOrDefault();

        if(spawnableObj == null)
        {
            spawnableObj = Instantiate(EnemyPrefab, spawnPosition, spawnRotation);
            EnemyBrainSelector brainSelector = spawnableObj.GetComponent<EnemyBrainSelector>();
            brainSelector.SwapBrain(Assets.GetGun(GunType), BrainType, Modifier, aIAgression);
        }
        else
        {
            spawnableObj.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObj);
            spawnableObj.SetActive(true);
        }

        return spawnableObj.GetComponent<EnemyBrain>();
    }

    public static void ReturnObjectToPool(GameObject obj, GunType GunType, EBrainType BrainType, EEnemyModifier Modifier)
    {
        PooledObjectInfo pool = ObjectPools.Find(p => p.GunType == GunType && p.BrainType == BrainType && p.Modifier == Modifier);

        if(pool == null)
        {
            Debug.LogError("Trying to release an object that is not pooled: " + obj.name);
        }
        else
        {
            Debug.Log(obj.name);
            obj.SetActive(false);
            pool.InactiveObjects.Add(obj);
        }
    }
}

public class PooledObjectInfo
{
    public GunType GunType;
    public EBrainType BrainType;
    public EEnemyModifier Modifier;
    public List<GameObject> InactiveObjects = new List<GameObject>();
}
