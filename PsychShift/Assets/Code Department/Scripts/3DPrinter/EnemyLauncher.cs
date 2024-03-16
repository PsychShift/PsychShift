using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLauncher : MonoBehaviour
{
    [SerializeField] private float force = 400;

    public void ShootEnemy()
    {
        BossFightBrain brain1 = EnemyPoolingManager.SpawnObject(Guns.GunType.Pistol, EBrainType.FinalBoss, EEnemyModifier.None, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity) as BossFightBrain;
        StartCoroutine(brain1.Launch(transform.forward, force));
    }
    public void ShootEnemy(Guns.GunType gunType, EEnemyModifier modifier)
    {
        BossFightBrain brain1 = EnemyPoolingManager.SpawnObject(gunType, EBrainType.FinalBoss, modifier, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity) as BossFightBrain;
        StartCoroutine(brain1.Launch(transform.forward, force));
    }

    // Dictionary<float, Guns.GunType> gunSpawnDict, Dictionary<float, EEnemyModifier> modSpawnDict
    public IEnumerator ShootEnemies(int numToShoot, float timeBetweenShots, List<Guns.GunType> guns, List<EEnemyModifier> mods)
    {
        int gunLen = guns.Count;
        int modLen = mods.Count;

        for(int i = 0; i < numToShoot; i++)
        {
            Guns.GunType type = guns[Random.Range(0, gunLen)];
            EEnemyModifier mod = mods[Random.Range(0, modLen)];
            ShootEnemy(type, mod);
            yield return new WaitForSeconds(timeBetweenShots);
        }
    }
}
