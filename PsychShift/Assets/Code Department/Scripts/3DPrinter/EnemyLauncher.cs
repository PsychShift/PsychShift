using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLauncher : MonoBehaviour
{
    [SerializeField] private float force = 400;

    public void ShootEnemy()
    {
        //BossFightBrain brain = EnemyPoolingManager.SpawnObject(Guns.GunType.Pistol, EBrainType.FinalBoss, new EEnemyModifier[] { EEnemyModifier.None }, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity) as BossFightBrain;
        //StartCoroutine(Test());
        Vector3 launchDir = transform.forward;
        BossFightBrain brain1 = EnemyPoolingManager.SpawnObject(Guns.GunType.Pistol, EBrainType.FinalBoss, EEnemyModifier.None, GameAssets.Instance.Agressions[0], transform.position, Quaternion.identity) as BossFightBrain;
        StartCoroutine(brain1.Launch(launchDir, force));
    }
}
