using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLauncher : MonoBehaviour
{
    void Start()
    {
        ShootEnemy();
    }
    public void ShootEnemy()
    {
        //BossFightBrain brain = EnemyPoolingManager.SpawnObject(Guns.GunType.Pistol, EBrainType.FinalBoss, new EEnemyModifier[] { EEnemyModifier.None }, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity) as BossFightBrain;
        //StartCoroutine(Test());
        Vector3 launchDir = transform.forward;
        BossFightBrain brain1 = EnemyPoolingManager.SpawnObject(Guns.GunType.Pistol, EBrainType.FinalBoss, EEnemyModifier.None, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity) as BossFightBrain;
        brain1.Launch(launchDir);
    }
    IEnumerator Test()
    {
        EnemyBrain brain1 = EnemyPoolingManager.SpawnObject(Guns.GunType.Pistol, EBrainType.Chase, EEnemyModifier.Explosive, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        EnemyPoolingManager.ReturnObjectToPool(brain1.gameObject, Guns.GunType.Pistol, EBrainType.Chase, EEnemyModifier.Explosive);
        yield return new WaitForSeconds(3f);
        EnemyPoolingManager.SpawnObject(Guns.GunType.Pistol, EBrainType.FinalBoss, EEnemyModifier.None, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity);
        
    }
}
