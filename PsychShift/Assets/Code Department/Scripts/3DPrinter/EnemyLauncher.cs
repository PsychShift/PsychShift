using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLauncher : MonoBehaviour
{
    [SerializeField] private float force = 400;
    [HideInInspector] public List<BossFightBrain> enemies;

    public void ShootEnemy(Guns.GunType gunType, EEnemyModifier modifier)
    {
        BossFightBrain brain = EnemyPoolingManager.SpawnObject(gunType, EBrainType.FinalBoss, modifier, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity) as BossFightBrain;
        StartCoroutine(brain.Launch(transform.forward, force));
        brain.EnemyHealth.OnDeath += HandleDeath;
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

    private void HandleDeath(Transform enemy)
    {
        BossFightBrain brain = enemy.GetComponent<BossFightBrain>();
        enemies.Remove(brain);

        brain.EnemyHealth.OnDeath -= HandleDeath;
    }

    private void OnDisable()
    {
        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].EnemyHealth.OnDeath -= HandleDeath;
        }

        enemies.Clear();
    }
}
