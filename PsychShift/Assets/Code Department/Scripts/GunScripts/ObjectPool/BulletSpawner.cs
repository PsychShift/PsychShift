using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class BulletSpawner : MonoBehaviour
{
    [SerializeField]
    private Bullet Prefab;
    [SerializeField]
    private BoxCollider SpawnArea;//box collider
    [SerializeField]
    private int BulletsPerSecond = 10;
    [SerializeField]
    private float Speed = 5f;
    [SerializeField]
    private bool UseObjectPool = false;//toggle pool in run time
    private float LastSpawnTime;//when to spawn another bullet
    private ObjectPool<Bullet> BulletPool;

    private void Awake()
    {
        BulletPool = new ObjectPool<Bullet>(CreatedPooledObject,OnTakeFromPool, OnReturnToPool, OnDestroyObject, false, 200, 100_000);
    }
    private Bullet CreatedPooledObject()
    {
        Bullet instance = Instantiate(Prefab, Vector3.zero, Quaternion.identity);
        instance.Disable+= ReturnObjectToPool;
        instance.gameObject.SetActive(false);

        return instance;
    }
    private void ReturnObjectToPool(Bullet Instance)
    {
        BulletPool.Release(Instance);

    }

    private void OnTakeFromPool(Bullet Instance)
    {
        Instance.gameObject.SetActive(true);
        SpawnBullet(Instance);
        Instance.transform.SetParent(transform, true);
    }

    private void OnReturnToPool(Bullet Instance)
    {
        Instance.gameObject.SetActive(false);
    }

    private void OnDestroyObject(Bullet Instance)
    {
        Destroy(Instance.gameObject);
    }

    private void Update() 
    {
        float delay = 1f / BulletsPerSecond;
        if(LastSpawnTime + delay < Time.time)
        {
            int bulletsToSpawnInFrame = Mathf.CeilToInt(Time.deltaTime/delay);//helps bullet spawning within frame
            while(bulletsToSpawnInFrame>0)
            {
                if(!UseObjectPool)
                {
                    Bullet instance = Instantiate(Prefab, Vector2.zero, Quaternion.identity);
                    instance.transform.SetParent(transform, true);

                    SpawnBullet(instance);
                }
                else
                {
                    BulletPool.Get();
                }
                bulletsToSpawnInFrame--;
            }
            LastSpawnTime = Time.time;
        }

            Debug.Log($"Total Pool Size: {BulletPool.CountAll}");
            Debug.Log($"Active Objects: {BulletPool.CountActive}"); 
    }
    private void SpawnBullet(Bullet instance)
    {
        Vector3 spawnLocation = new Vector3(
                        SpawnArea.transform.position.x +SpawnArea.center.x + Random.Range(-1 * SpawnArea.bounds.extents.x,SpawnArea.bounds.extents.x),
                        SpawnArea.transform.position.y +SpawnArea.center.y + Random.Range(-1 * SpawnArea.bounds.extents.y,SpawnArea.bounds.extents.y),
                        SpawnArea.transform.position.z +SpawnArea.center.z + Random.Range(-1 * SpawnArea.bounds.extents.z,SpawnArea.bounds.extents.z));

        instance.transform.position = spawnLocation;

        instance.Shoot(spawnLocation, SpawnArea.transform.forward, Speed);
    }
}
